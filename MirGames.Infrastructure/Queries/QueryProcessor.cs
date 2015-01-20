// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// Implementation of the query processor.
    /// </summary>
    internal sealed class QueryProcessor : IQueryProcessor
    {
        /// <summary>
        /// The command handlers.
        /// </summary>
        private readonly Lazy<ILookup<Type, IQueryHandler>> queryHandlers;

        /// <summary>
        /// The query handler decorators.
        /// </summary>
        private readonly IEnumerable<IQueryHandlerDecorator> queryHandlerDecorators;

        /// <summary>
        /// The claims principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> claimsPrincipalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessor" /> class.
        /// </summary>
        /// <param name="queryHandlers">The query handlers.</param>
        /// <param name="queryHandlerDecorators">The query handler decorators.</param>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        public QueryProcessor(
            Lazy<IEnumerable<IQueryHandler>> queryHandlers,
            IEnumerable<IQueryHandlerDecorator> queryHandlerDecorators,
            Func<ClaimsPrincipal> claimsPrincipalProvider)
        {
            Contract.Requires(queryHandlers != null);
            Contract.Requires(claimsPrincipalProvider != null);
            Contract.Requires(queryHandlerDecorators != null);

            this.queryHandlers =
                new Lazy<ILookup<Type, IQueryHandler>>(() => queryHandlers.Value.ToLookup(k => k.QueryType));
            this.queryHandlerDecorators = queryHandlerDecorators.OrderBy(q => q.Order).ToList();
            this.claimsPrincipalProvider = claimsPrincipalProvider;
        }

        /// <inheritdoc />
        public int GetItemsCount(Query query)
        {
            Contract.Requires(query != null);

            var claimsPrincipal = this.GetPrincipal();
            var registeredQueryHandlers = this.GetQueryHandlers(query);

            try
            {
                return registeredQueryHandlers.Sum(h => h.GetItemsCount(query, claimsPrincipal));
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new QueryProcessingFailedException(query, e);
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> Process<T>(Query query, PaginationSettings pagination = null)
        {
            Contract.Requires(query != null);

            var claimsPrincipal = this.GetPrincipal();
            var registeredQueryHandlers = this.GetQueryHandlers(query);

            try
            {
                return registeredQueryHandlers.SelectMany(h => (IEnumerable<T>)h.Execute(query, claimsPrincipal, pagination)).EnsureCollection();
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new QueryProcessingFailedException(query, e);
            }
        }

        /// <inheritdoc />
        public Task<ICollection<T>> ProcessAsync<T>(Query query, PaginationSettings pagination = null)
        {
            Contract.Requires(query != null);

            return Task.Factory.StartNew(() => (ICollection<T>)this.Process<T>(query, pagination).ToList());
        }

        /// <summary>
        /// Gets the principal.
        /// </summary>
        /// <returns>The principal.</returns>
        private ClaimsPrincipal GetPrincipal()
        {
            return this.claimsPrincipalProvider.Invoke();
        }

        /// <summary>
        /// Decorates the query handler.
        /// </summary>
        /// <param name="queryHandler">The query handler.</param>
        /// <returns>The decorated query handler.</returns>
        private IQueryHandler DecorateQueryHandler(IQueryHandler queryHandler)
        {
            return this.queryHandlerDecorators.Aggregate(queryHandler, (current, handlerDecorator) => handlerDecorator.Decorate(current));
        }

        /// <summary>
        /// Gets the query handlers.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query handlers that could process the query.</returns>
        private IEnumerable<IQueryHandler> GetQueryHandlers(Query query)
        {
            var queryType = query.GetType();
            var handlers = this.queryHandlers.Value[queryType].EnsureCollection();

            if (handlers.Any())
            {
                return handlers.Select(this.DecorateQueryHandler);
            }
            
            throw new InvalidOperationException("Query handler for queries of type " + queryType.FullName + " have not been found.");
        }
    }
}
