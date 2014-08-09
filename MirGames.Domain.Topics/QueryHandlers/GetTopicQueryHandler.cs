// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
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
    internal sealed class GetTopicQueryHandler : SingleItemQueryHandler<GetTopicQuery, TopicViewModel>
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
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The blog resolver.
        /// </summary>
        private readonly IBlogResolver blogResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textProcessor">The text processor.</param>
        /// <param name="blogResolver">The blog resolver.</param>
        public GetTopicQueryHandler(
            IAuthorizationManager authorizationManager,
            IQueryProcessor queryProcessor,
            ITextProcessor textProcessor,
            IBlogResolver blogResolver)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(textProcessor != null);
            Contract.Requires(blogResolver != null);

            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
            this.textProcessor = textProcessor;
            this.blogResolver = blogResolver;
        }

        /// <inheritdoc />
        public override TopicViewModel Execute(IReadContext readContext, GetTopicQuery query, ClaimsPrincipal principal)
        {
            TopicViewModel topic = readContext
                .Query<Topic>()
                .Where(t => t.Id == query.TopicId)
                .Select(t => new TopicViewModel
                    {
                        Id = t.Id,
                        Text = t.Content.TopicText,
                        CommentsCount = t.CountComment,
                        Title = t.TopicTitle,
                        Author = new AuthorViewModel { Id = t.AuthorId },
                        Blog = new BlogViewModel { BlogId = t.BlogId },
                        TagsList = t.TagsList,
                        CreationDate = t.CreationDate
                    })
                .FirstOrDefault();

            if (topic == null)
            {
                return null;
            }

            var newTopics =
                this.queryProcessor.GetItemsCount(
                    new GetNotificationsQuery().WithFilter<NewBlogTopicNotification>(n => n.TopicId == topic.Id));

            var newComments =
                this.queryProcessor.Process(
                    new GetNotificationsQuery().WithFilter<NewTopicCommentNotification>(n => n.TopicId == topic.Id))
                    .Select(t => (NewTopicCommentNotification)t.Data)
                    .Select(t => t.CommentId)
                    .ToArray();

            topic.Blog = this.blogResolver.Resolve(topic.Blog);
            this.queryProcessor.Process(new ResolveAuthorsQuery { Authors = new[] { topic.Author } });

            topic.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "Topic", topic.Id);
            topic.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "Topic", topic.Id);
            topic.CanBeCommented = this.authorizationManager.CheckAccess(principal, "Comment", "Topic", topic.Id);
            topic.Comments = this.GetComments(readContext, query, principal, newComments);
            topic.IsRead = (newTopics + newComments.Length) == 0;

            return topic;
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="newComments"></param>
        /// <returns>
        /// The comments list.
        /// </returns>
        private IEnumerable<CommentViewModel> GetComments(
            IReadContext readContext,
            GetTopicQuery query,
            ClaimsPrincipal principal,
            int[] newComments)
        {
            var comments = readContext
                .Query<Comment>()
                .Where(c => c.TopicId == query.TopicId)
                .OrderBy(c => c.Date)
                .ToList();

            var commentViewModels = comments
                .Select(
                    c => new CommentViewModel
                    {
                        Author = new AuthorViewModel
                        {
                            Id = c.UserId,
                            Login = c.UserLogin
                        },
                        CreationDate = c.Date,
                        UpdatedDate = c.UpdatedDate,
                        Text = c.Text ?? this.textProcessor.GetHtml(c.SourceText),
                        Id = c.CommentId,
                        TopicId = c.TopicId,
                        CanBeDeleted =
                            this.authorizationManager.CheckAccess(principal, "Delete", "Comment", c.CommentId),
                        CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "Comment", c.CommentId),
                        IsRead = !newComments.Contains(c.CommentId)
                    })
                .ToList();

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                {
                    Authors = commentViewModels.Select(c => c.Author)
                });

            return commentViewModels;
        }
    }
}