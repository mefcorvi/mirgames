// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TraceQueryHandlerDecorator.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.QueryHandlerDecorators
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Traces the query execution time.
    /// </summary>
    internal sealed class TraceQueryHandlerDecorator : IQueryHandlerDecorator
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceQueryHandlerDecorator"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="eventLog">The event log.</param>
        public TraceQueryHandlerDecorator(ISettings settings, IEventLog eventLog)
        {
            Contract.Requires(eventLog != null);
            Contract.Requires(settings != null);

            this.settings = settings;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        public float Order
        {
            get { return float.MaxValue; }
        }

        /// <inheritdoc />
        public IQueryHandler Decorate(IQueryHandler queryHandler)
        {
            bool isTraceEnabled = this.settings.GetValue("QueryProcessor.TraceEnabled", false);
            return isTraceEnabled ? new TraceQueryHandler(queryHandler, this.eventLog) : queryHandler;
        }

        /// <summary>
        /// The decorated handler.
        /// </summary>
        private class TraceQueryHandler : IQueryHandler
        {
            /// <summary>
            /// The inner.
            /// </summary>
            private readonly IQueryHandler inner;

            /// <summary>
            /// The event log.
            /// </summary>
            private readonly IEventLog eventLog;

            /// <summary>
            /// Initializes a new instance of the <see cref="TraceQueryHandler" /> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="eventLog">The event log.</param>
            public TraceQueryHandler(IQueryHandler inner, IEventLog eventLog)
            {
                Contract.Requires(inner != null);
                this.inner = inner;
                this.eventLog = eventLog;
            }

            /// <inheritdoc />
            public Type QueryType
            {
                get { return this.inner.QueryType; }
            }

            /// <inheritdoc />
            public IEnumerable Execute(Query query, ClaimsPrincipal principal, PaginationSettings pagination)
            {
                var sw = new Stopwatch();
                sw.Start();

                var result = this.inner.Execute(query, principal, pagination);

                sw.Stop();
                this.TraceQuery(query, sw.ElapsedMilliseconds);

                return result;
            }

            /// <inheritdoc />
            public int GetItemsCount(Query query, ClaimsPrincipal principal)
            {
                var sw = new Stopwatch();
                sw.Start();

                var result = this.inner.GetItemsCount(query, principal);

                sw.Stop();
                this.TraceQuery(query, sw.ElapsedMilliseconds);

                return result;
            }

            /// <summary>
            /// Traces the query.
            /// </summary>
            /// <param name="query">The query.</param>
            /// <param name="elapsedMilliseconds">The elapsed milliseconds.</param>
            private void TraceQuery(Query query, long elapsedMilliseconds)
            {
                var queryType = query.GetType();
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
    }
}
