// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetEventLogQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Tools.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Tools.Entities;
    using MirGames.Domain.Tools.Queries;
    using MirGames.Domain.Tools.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetTopicsQuery.
    /// </summary>
    internal sealed class GetEventLogQueryHandler : QueryHandler<GetEventLogQuery, EventLogViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEventLogQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetEventLogQueryHandler(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        protected override IEnumerable<EventLogViewModel> Execute(IReadContext readContext, GetEventLogQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            this.authorizationManager.EnsureAccess(principal, "View", new EventLog());

            var topics = this.GetItemsQueryable(readContext, query);
            return this.ApplyPagination(topics, pagination).ToList().Select(this.Convert);
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetEventLogQuery query, ClaimsPrincipal principal)
        {
            return this.GetItemsQueryable(readContext, query).Count();
        }

        /// <summary>
        /// Gets the topics query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The prepared query.
        /// </returns>
        private IQueryable<EventLog> GetItemsQueryable(IReadContext readContext, GetEventLogQuery query)
        {
            IQueryable<EventLog> eventLogs = readContext.Query<EventLog>().Where(e => e.Date >= query.From);

            if (query.To.HasValue)
            {
                eventLogs = eventLogs.Where(e => e.Date <= query.To);
            }

            if (query.LogType.HasValue)
            {
                eventLogs = eventLogs.Where(e => e.EventLogType == query.LogType.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                eventLogs = eventLogs.Where(e => e.Login.StartsWith(query.UserName));
            }

            if (!string.IsNullOrWhiteSpace(query.Source))
            {
                eventLogs = eventLogs.Where(e => e.Source.Contains(query.Source));
            }

            if (!string.IsNullOrWhiteSpace(query.Message))
            {
                eventLogs = eventLogs.Where(e => e.Message.Contains(query.Message));
            }

            return eventLogs.OrderByDescending(t => t.Date);
        }

        /// <summary>
        /// Converts the specified topic.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <returns>The specified list item.</returns>
        private EventLogViewModel Convert(EventLog eventLog)
        {
            return new EventLogViewModel
                {
                    Details = eventLog.Details,
                    EventLogType = eventLog.EventLogType,
                    Id = eventLog.Id,
                    Login = eventLog.Login,
                    Message = eventLog.Message,
                    Source = eventLog.Source,
                    Date = eventLog.Date
                };
        }
    }
}