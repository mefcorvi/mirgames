// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUnreadForumsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    public sealed class GetUnreadForumsQueryHandler : QueryHandler<GetUnreadForumsQuery, int>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUnreadForumsQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetUnreadForumsQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetUnreadForumsQuery query, ClaimsPrincipal principal)
        {
            return this.GetNotifications().Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<int> Execute(
            GetUnreadForumsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            return this.GetNotifications().EnsureCollection();
        }

        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <returns>The notifications.</returns>
        private IEnumerable<int> GetNotifications()
        {
            return this.queryProcessor
                       .Process(new GetNotificationsQuery
                       {
                           IsRead = false,
                           Filter = n => n is NewForumAnswerNotification || n is NewForumTopicNotification
                       })
                       .Select(n => ((ForumTopicNotificationData)n.Data).ForumId)
                       .Distinct();
        }
    }
}
