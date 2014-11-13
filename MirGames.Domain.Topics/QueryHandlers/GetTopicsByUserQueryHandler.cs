// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicsByUserQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.Services;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetTopicQuery.
    /// </summary>
    internal sealed class GetTopicsByUserQueryHandler : QueryHandler<GetTopicsByUserQuery, TopicsListItem>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The blog resolver.
        /// </summary>
        private readonly IBlogResolver blogResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsByUserQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="blogResolver">The blog resolver.</param>
        public GetTopicsByUserQueryHandler(IAuthorizationManager authorizationManager, IQueryProcessor queryProcessor, IBlogResolver blogResolver)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(blogResolver != null);

            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
            this.blogResolver = blogResolver;
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(IReadContext readContext, GetTopicsByUserQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var author = this.queryProcessor.Process(
                new GetAuthorQuery
                    {
                        UserId = query.UserId
                    });

            var topics =
                this.ApplyPagination(
                    readContext
                        .Query<Topic>()
                        .Include(t => t.Content)
                        .Where(t => t.AuthorId == query.UserId && t.IsPublished)
                        .OrderByDescending(t => t.CreationDate),
                    pagination)
                    .ToList()
                    .Select(t => this.GetTopic(t, author, principal))
                    .ToList();

            var topicIdentifiers = topics.Select(t => t.TopicId).ToArray();

            var newTopics =
                this.queryProcessor.Process(
                    new GetNotificationsQuery().WithFilter<NewBlogTopicNotification>(
                        n => topicIdentifiers.Contains(n.TopicId)))
                    .Select(t => (NewBlogTopicNotification)t.Data)
                    .Select(t => t.TopicId)
                    .ToArray();

            var newComments =
                this.queryProcessor.Process(
                    new GetNotificationsQuery().WithFilter<NewTopicCommentNotification>(
                        n => topicIdentifiers.Contains(n.TopicId)))
                    .Select(t => (NewTopicCommentNotification)t.Data)
                    .GroupBy(t => t.TopicId)
                    .ToDictionary(t => t.Key, t => t.Count());

            foreach (var topicsListItem in topics)
            {
                topicsListItem.UnreadCommentsCount = newComments.ContainsKey(topicsListItem.TopicId)
                                                         ? newComments[topicsListItem.TopicId]
                                                         : 0;
                topicsListItem.IsRead = !newTopics.Contains(topicsListItem.TopicId);
                topicsListItem.Blog = this.blogResolver.Resolve(topicsListItem.Blog);
            }

            return topics;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetTopicsByUserQuery query, ClaimsPrincipal principal)
        {
            return readContext.Query<Topic>().Count(t => t.AuthorId == query.UserId && t.IsPublished);
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="author">The author.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The topics list item.</returns>
        private TopicsListItem GetTopic(Topic topic, AuthorViewModel author, ClaimsPrincipal principal)
        {
            return new TopicsListItem
                {
                    Author = author,
                    Blog = new BlogViewModel
                    {
                        BlogId = topic.BlogId
                    },
                    CanBeCommented = this.authorizationManager.CheckAccess(principal, "Comment", "Topic", topic.Id),
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "Topic", topic.Id),
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "Topic", topic.Id),
                    CreationDate = topic.CreationDate,
                    CommentsCount = topic.CountComment,
                    ShortText = topic.Content.TopicTextShort,
                    Tags = topic.TagsList,
                    TopicId = topic.Id,
                    Title = topic.TopicTitle,
                    IsTutorial = topic.IsTutorial,
                    IsRepost = topic.IsRepost,
                    SourceAuthor = topic.SourceAuthor,
                    SourceLink = topic.SourceLink,
                    ReadMoreText = topic.CutText,
                    IsMicroTopic = topic.IsMicroTopic
                };
        }
    }
}