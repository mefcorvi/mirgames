// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNewForumAnswserNotificationDetailsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.QueryHandlers.Notifications
{
    using System.Collections.Generic;
    using System.Data.Entity;
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
    internal sealed class GetNewForumAnswserNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
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
        /// Initializes a new instance of the <see cref="GetNewForumAnswserNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetNewForumAnswserNotificationDetailsQueryHandler(IReadContextFactory readContextFactory, IQueryProcessor queryProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(queryProcessor != null);

            this.readContextFactory = readContextFactory;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetNotificationDetailsQuery query, ClaimsPrincipal principal)
        {
            return query.Notifications.Count(n => n is NewForumAnswerNotification);
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<NewForumAnswerNotification>().ToArray();
            var affectedPosts = notifications.Select(n => n.PostId).ToArray();

            if (!affectedPosts.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            List<NewForumAnswerNotificationDetailsViewModel> posts;
            using (var readContext = this.readContextFactory.Create())
            {
                posts = readContext
                    .Query<ForumPost>()
                    .Include(p => p.Topic)
                    .Where(post => affectedPosts.Contains(post.PostId))
                    .Select(post => new NewForumAnswerNotificationDetailsViewModel
                    {
                        Author = new AuthorViewModel { Id = post.AuthorId, Login = post.AuthorLogin },
                        PostDate = post.CreatedDate,
                        PostId = post.PostId,
                        TopicId = post.TopicId,
                        PostText = post.Text,
                        TopicTitle = post.Topic.Title,
                        Forum = new ForumViewModel
                        {
                            ForumId = post.Topic.ForumId
                        }
                    })
                    .ToList();
            }

            var forums = this.queryProcessor.Process(new GetForumsQuery()).ToDictionary(f => f.ForumId);
            posts.ForEach(post =>
            {
                post.Forum = forums[post.Forum.ForumId];
            });

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = posts.Select(p => p.Author).ToArray()
            });

            return posts.CopyBaseNotificationData(notifications, model => model.PostId, notification => notification.PostId).ToList();
        }
    }
}
