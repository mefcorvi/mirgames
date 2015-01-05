// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNewCommitNotificationDetailsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.QueryHandlers.Notifications
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Extensions;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Notifications;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the <see cref="GetNotificationDetailsQuery"/> query.
    /// </summary>
    internal sealed class GetNewCommitNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNewCommitNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetNewCommitNotificationDetailsQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetNotificationDetailsQuery query, ClaimsPrincipal principal)
        {
            return query.Notifications.Count(n => n is NewCommitNotification);
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<NewCommitNotification>().ToArray();
            var affectedProjects = notifications.Select(n => n.ProjectAlias).ToArray();

            if (!affectedProjects.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            var commits = new List<NewCommitNotificationDetailsViewModel>();

            foreach (var notification in notifications)
            {
                var commit = this.queryProcessor.Process(new GetWipProjectCommitsQuery
                {
                    Alias = notification.ProjectAlias,
                    CommitId = notification.CommitId
                }).FirstOrDefault();

                if (commit != null)
                {
                    commits.Add(new NewCommitNotificationDetailsViewModel
                    {
                        Author = new AuthorViewModel
                        {
                            Id = notification.ComiteerId
                        },
                        CommitId = commit.Id,
                        Message = commit.Message,
                        ProjectAlias = notification.ProjectAlias
                    });
                }
            }

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = commits.Select(p => p.Author).ToArray()
            });

            return commits.CopyBaseNotificationData(notifications, model => model.CommitId, notification => notification.CommitId).ToList();
        }
    }
}
