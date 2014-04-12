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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;

    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Logging;

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
        /// The query item post processors.
        /// </summary>
        private readonly ILookup<Type, IQueryItemPostProcessor> queryItemPostProcessors;

        /// <summary>
        /// The claims principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> claimsPrincipalProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessor" /> class.
        /// </summary>
        /// <param name="queryHandlers">The query handlers.</param>
        /// <param name="queryItemPostProcessors">The query item post processors.</param>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="eventLog">The event log.</param>
        public QueryProcessor(Lazy<IEnumerable<IQueryHandler>> queryHandlers, IEnumerable<IQueryItemPostProcessor> queryItemPostProcessors, Func<ClaimsPrincipal> claimsPrincipalProvider, IReadContextFactory readContextFactory, ISettings settings, IEventLog eventLog)
        {
            Contract.Requires(queryHandlers != null);
            Contract.Requires(queryItemPostProcessors != null);
            Contract.Requires(claimsPrincipalProvider != null);
            Contract.Requires(settings != null);
            Contract.Requires(eventLog != null);

            this.queryHandlers = new Lazy<Dictionary<Type, IQueryHandler>>(() => queryHandlers.Value.ToDictionary(c => c.QueryType));
            this.queryItemPostProcessors = queryItemPostProcessors.ToLookup(c => c.ItemType);
            this.claimsPrincipalProvider = claimsPrincipalProvider;
            this.readContextFactory = readContextFactory;
            this.settings = settings;
            this.eventLog = eventLog;
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
                (handler, principal, readContext) => ((ISingleItemQueryHandler<T>)handler).Execute(readContext, query, principal));
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
                try
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    T result;
                    using (var readContext = this.readContextFactory.Create())
                    {
                        result = queryAction(this.queryHandlers.Value[queryType], claimsPrincipal, readContext);
                    }

                    sw.Stop();

                    this.TraceQuery(query, queryType, sw.ElapsedMilliseconds);

                    if (!Equals(result, null))
                    {
                        this.PostProcessing(result);
                    }

                    return result;
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

        /// <summary>
        /// Traces the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="queryType">Type of the query.</param>
        /// <param name="elapsedMilliseconds">The elapsed milliseconds.</param>
        private void TraceQuery(Query query, Type queryType, long elapsedMilliseconds)
        {
            bool isTraceEnabled = this.settings.GetValue("QueryProcessor.TraceEnabled", false);
            if (isTraceEnabled)
            {
                var attributes = queryType.GetCustomAttributes(typeof(DisableTracingAttribute), true).Cast<DisableTracingAttribute>();

                if (attributes.Any())
                {
                    return;
                }

                this.eventLog.Log(
                    EventLogType.Verbose,
                    "QueryProcessor",
                    string.Format("Execution of \"{0}\" have been completed in {1} ms.", queryType.Name, elapsedMilliseconds),
                    query);
            }
        }

        /// <summary>
        /// Post processing of the result.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="result">The result.</param>
        private void PostProcessing<T>(T result)
        {
            foreach (var processor in this.queryItemPostProcessors[result.GetType()])
            {
                processor.Process(result);
            }
        }
    }
}
