// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNewBlogTopicNotificationDetailsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.QueryHandlers.Notifications
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Extensions;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Topics.Services;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the <see cref="GetNewBlogTopicNotificationDetailsQueryHandler"/> query.
    /// </summary>
    internal sealed class GetNewBlogTopicNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
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
        /// The blog resolver.
        /// </summary>
        private readonly IBlogResolver blogResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNewBlogTopicNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="blogResolver">The blog resolver.</param>
        public GetNewBlogTopicNotificationDetailsQueryHandler(
            IReadContextFactory readContextFactory,
            IQueryProcessor queryProcessor,
            IBlogResolver blogResolver)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(blogResolver != null);

            this.readContextFactory = readContextFactory;
            this.queryProcessor = queryProcessor;
            this.blogResolver = blogResolver;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetNotificationDetailsQuery query, ClaimsPrincipal principal)
        {
            return query.Notifications.Count(n => n is NewBlogTopicNotification);
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<NewBlogTopicNotification>().ToArray();
            var affectedPosts = notifications.Select(n => n.TopicId).ToArray();

            if (!affectedPosts.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            List<NewBlogTopicNotificationDetailsViewModel> topics;
            using (var readContext = this.readContextFactory.Create())
            {
                topics = readContext
                    .Query<Topic>()
                    .Where(post => affectedPosts.Contains(post.Id))
                    .Select(t => new NewBlogTopicNotificationDetailsViewModel
                    {
                        TopicId = t.Id,
                        Text = t.Content.TopicTextShort,
                        CommentsCount = t.CountComment,
                        Title = t.TopicTitle,
                        Author = new AuthorViewModel { Id = t.AuthorId },
                        Blog = new BlogViewModel { BlogId = t.BlogId },
                        CreationDate = t.CreationDate,
                        IsMicroTopic = t.IsMicroTopic
                    })
                    .ToList();
            }

            topics.ForEach(topic =>
            {
                topic.Blog = this.blogResolver.Resolve(topic.Blog);
            });

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = topics.Select(p => p.Author).ToArray()
            });

            return topics.CopyBaseNotificationData(notifications, model => model.TopicId, notification => notification.TopicId).ToList();
        }
    }
}
