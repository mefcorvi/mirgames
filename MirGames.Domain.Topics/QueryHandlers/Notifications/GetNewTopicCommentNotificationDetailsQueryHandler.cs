// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNewTopicCommentNotificationDetailsQueryHandler.cs">
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
    /// Handles the <see cref="GetNewTopicCommentNotificationDetailsQueryHandler"/> query.
    /// </summary>
    internal sealed class GetNewTopicCommentNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
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
        /// Initializes a new instance of the <see cref="GetNewTopicCommentNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="blogResolver">The blog resolver.</param>
        public GetNewTopicCommentNotificationDetailsQueryHandler(
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
            return query.Notifications.Count(n => n is NewTopicCommentNotification);
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<NewTopicCommentNotification>().ToArray();
            var affectedComments = notifications.Select(n => n.CommentId).ToArray();
            var affectedTopics = notifications.Select(n => n.TopicId).ToArray();

            if (!affectedComments.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            List<NewTopicCommentNotificationDetailsViewModel> comments;
            List<Topic> topics;
            
            using (var readContext = this.readContextFactory.Create())
            {
                comments = readContext
                    .Query<Comment>()
                    .Where(comment => affectedComments.Contains(comment.CommentId))
                    .Select(comment => new NewTopicCommentNotificationDetailsViewModel
                    {
                        CommentId = comment.CommentId,
                        TopicId = comment.TopicId,
                        Text = comment.Text,
                        Author = new AuthorViewModel { Id = comment.UserId },
                        CreationDate = comment.Date
                    })
                    .ToList();

                topics = readContext.Query<Topic>().Where(topic => affectedTopics.Contains(topic.Id)).ToList();
            }

            comments.ForEach(comment =>
            {
                var topic = topics.Single(t => t.Id == comment.TopicId);
                comment.Blog = this.blogResolver.Resolve(new BlogViewModel { BlogId = topic.BlogId });
                comment.Title = topic.TopicTitle;
            });

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = comments.Select(p => p.Author).ToArray()
            });

            return comments.CopyBaseNotificationData(notifications, model => model.CommentId, notification => notification.CommentId).ToList();
        }
    }
}
