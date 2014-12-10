// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumTopicQueryHandler : SingleItemQueryHandler<GetForumTopicQuery, ForumTopicViewModel>
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
        /// Initializes a new instance of the <see cref="GetForumTopicQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetForumTopicQueryHandler(IAuthorizationManager authorizationManager, IQueryProcessor queryProcessor)
        {
            Contract.Assert(authorizationManager != null);
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override ForumTopicViewModel Execute(IReadContext readContext, GetForumTopicQuery query, ClaimsPrincipal principal)
        {
            var topic = this.GetTopic(readContext, query);

            if (topic == null)
            {
                return null;
            }

            var topicViewModel = new ForumTopicViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = topic.AuthorId,
                            Login = topic.AuthorLogin
                        },
                    AuthorIp = topic.AuthorIp,
                    CreatedDate = topic.CreatedDate,
                    Title = topic.Title,
                    TopicId = topic.TopicId,
                    TagsList = topic.TagsList,
                    UpdatedDate = topic.UpdatedDate
                };

            var post = readContext.Query<ForumPost>().First(p => p.TopicId == topic.TopicId && p.IsStartPost);

            topicViewModel.StartPost = new ForumPostsListItemViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = post.AuthorId,
                            Login = post.AuthorLogin
                        },
                    AuthorIP = post.AuthorIP,
                    CreatedDate = post.CreatedDate,
                    IsHidden = post.IsHidden,
                    Text = post.Text,
                    TopicId = post.TopicId,
                    PostId = post.PostId,
                    UpdatedDate = post.UpdatedDate,
                    IsRead = true,
                    IsFirstPost = post.IsStartPost,
                    Index = 1,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "ForumPost", post.PostId) && !post.IsStartPost,
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "ForumPost", post.PostId),
                    CanBeVoted = principal.IsInRole("User") && post.AuthorId != principal.GetUserId()
                };

            topicViewModel.CanBeAnswered = this.authorizationManager.CheckAccess(principal, "Reply", "ForumTopic", topic.TopicId);
            topicViewModel.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "ForumTopic", topic.TopicId);
            topicViewModel.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "ForumTopic", topic.TopicId);

            if (!principal.Identity.IsAuthenticated)
            {
                topicViewModel.IsRead = true;
            }
            else
            {
                var newTopicsNotifications =
                    this.queryProcessor
                        .GetItemsCount(
                            new GetNotificationsQuery().WithFilter<NewForumTopicNotification>(
                                n => n.TopicId == topicViewModel.TopicId));

                var answerNotifications =
                    this.queryProcessor.GetItemsCount(
                        new GetNotificationsQuery().WithFilter<NewForumAnswerNotification>(
                            n => n.TopicId == topicViewModel.TopicId));

                topicViewModel.IsRead = (newTopicsNotifications + answerNotifications) == 0;
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                {
                    Authors = new[] { topicViewModel.Author, topicViewModel.StartPost.Author }
                });

            var forums = this.queryProcessor.Process(new GetForumsQuery()).EnsureCollection();
            topicViewModel.Forum = forums.FirstOrDefault(f => f.ForumId == topic.ForumId);

            return topicViewModel;
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The topic.</returns>
        private ForumTopic GetTopic(IReadContext readContext, GetForumTopicQuery query)
        {
            return readContext.Query<ForumTopic>().FirstOrDefault(t => t.TopicId == query.TopicId);
        }
    }
}