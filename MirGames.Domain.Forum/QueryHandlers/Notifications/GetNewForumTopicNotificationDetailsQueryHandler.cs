// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNewForumTopicNotificationDetailsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.QueryHandlers.Notifications
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Notifications.Extensions;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the <see cref="GetNotificationDetailsQuery"/> query.
    /// </summary>
    internal sealed class GetNewForumTopicNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNewForumTopicNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetNewForumTopicNotificationDetailsQueryHandler(IReadContextFactory readContextFactory, IQueryProcessor queryProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(queryProcessor != null);

            this.readContextFactory = readContextFactory;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetNotificationDetailsQuery query, ClaimsPrincipal principal)
        {
            return query.Notifications.OfType<NewForumTopicNotification>().Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<NewForumTopicNotification>().ToArray();
            var affectedTopics = notifications.Select(n => n.TopicId).ToArray();

            if (!affectedTopics.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            var forums = this.queryProcessor.Process(new GetForumsQuery()).ToDictionary(f => f.ForumId);

            List<NewForumTopicNotificationDetailsViewModel> topics;
            using (var readContext = this.readContextFactory.Create())
            {
                topics = readContext
                    .Query<ForumTopic>()
                    .Where(topic => affectedTopics.Contains(topic.TopicId))
                    .Select(topic => new NewForumTopicNotificationDetailsViewModel
                    {
                        Author = new AuthorViewModel { Id = topic.AuthorId, Login = topic.AuthorLogin },
                        Forum = new ForumViewModel { ForumId = topic.ForumId },
                        Text = topic.ShortDescription,
                        TopicDate = topic.CreatedDate,
                        TopicId = topic.TopicId,
                        TopicTitle = topic.Title,
                        NotificationDate = topic.CreatedDate
                    })
                    .ToList();
            }

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = topics.Select(p => p.Author).ToArray()
            });

            topics.ForEach(topic =>
            {
                topic.Forum = forums[topic.Forum.ForumId];
            });

            return topics.CopyBaseNotificationData(notifications, model => model.TopicId, notification => notification.TopicId).ToList();
        }
    }
}
