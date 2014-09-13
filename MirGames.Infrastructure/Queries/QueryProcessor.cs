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
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;

    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// Implementation of the query processor.
    /// </summary>
    internal sealed class QueryProcessor : IQueryProcessor
    {
        /// <summary>
        /// The command handlers.
        /// </summary>
        private readonly Lazy<Dictionary<Type, IQueryHandler>> queryHandlers;

        /// <summary>
        /// The query handler decorators.
        /// </summary>
        private readonly IEnumerable<IQueryHandlerDecorator> queryHandlerDecorators;

        /// <summary>
        /// The claims principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> claimsPrincipalProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessor" /> class.
        /// </summary>
        /// <param name="queryHandlers">The query handlers.</param>
        /// <param name="queryHandlerDecorators">The query handler decorators.</param>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public QueryProcessor(
            Lazy<IEnumerable<IQueryHandler>> queryHandlers,
            IEnumerable<IQueryHandlerDecorator> queryHandlerDecorators,
            Func<ClaimsPrincipal> claimsPrincipalProvider,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryHandlers != null);
            Contract.Requires(claimsPrincipalProvider != null);
            Contract.Requires(queryHandlerDecorators != null);

            this.queryHandlers =
                new Lazy<Dictionary<Type, IQueryHandler>>(() => queryHandlers.Value.ToDictionary(c => c.QueryType));
            this.queryHandlerDecorators = queryHandlerDecorators.OrderBy(q => q.Order).ToList();
            this.claimsPrincipalProvider = claimsPrincipalProvider;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        public IEnumerable<T> Process<T>(Query<T> query, PaginationSettings pagination = null)
        {
            Contract.Requires(query != null);

            return this.CallQueryHandler(
                query,
                (handler, principal, readContext) => handler.Execute(readContext, query, principal, pagination).Cast<T>().EnsureCollection());
        }

        /// <inheritdoc />
        public IEnumerable<object> Process(Query query, PaginationSettings pagination)
        {
            Contract.Requires(query != null);

            return this.CallQueryHandler(
                query,
                (handler, principal, readContext) => handler.Execute(readContext, query, principal, pagination).Cast<object>().EnsureCollection());
        }

        /// <inheritdoc />
        public int GetItemsCount(Query query)
        {
            Contract.Requires(query != null);

            return this.CallQueryHandler(
                query,
                (handler, principal, readContext) => handler.GetItemsCount(readContext, query, principal));
        }

        /// <inheritdoc />
        public T Process<T>(SingleItemQuery<T> query)
        {
            Contract.Requires(query != null);

            return this.CallQueryHandler(
                query,
                (handler, principal, readContext) => handler.Execute(readContext, query, principal, null).Cast<T>().SingleOrDefault());
        }

        /// <summary>
        /// Invokes the query.
        /// </summary>
        /// <typeparam name="T">The type of result.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="queryAction">The query action.</param>
        /// <returns>
        /// The result.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private T CallQueryHandler<T>(Query query, Func<IQueryHandler, ClaimsPrincipal, IReadContext, T> queryAction)
        {
            Contract.Requires(query != null);
            Contract.Requires(queryAction != null);

            var claimsPrincipal = this.claimsPrincipalProvider.Invoke();
            var queryType = query.GetType();

            if (this.queryHandlers.Value.ContainsKey(queryType))
            {
                var queryHandler = this.queryHandlers.Value[queryType];
                queryHandler = this.queryHandlerDecorators.Aggregate(queryHandler, (current, handlerDecorator) => handlerDecorator.Decorate(current));

                try
                {
                    using (var readContext = this.readContextFactory.Create())
                    {
                        return queryAction(queryHandler, claimsPrincipal, readContext);
                    }
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

            throw new InvalidOperationException("Query handler for queries of type " + queryType.FullName + " have not been found.");
        }
    }
}
