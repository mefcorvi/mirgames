// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicPostsQueryHandler.cs">
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
    public class GetForumTopicPostsQueryHandler : QueryHandler<GetForumTopicPostsQuery, ForumPostsListItemViewModel>
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
        /// Initializes a new instance of the <see cref="GetForumTopicPostsQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetForumTopicPostsQueryHandler(IAuthorizationManager authorizationManager, IQueryProcessor queryProcessor)
        {
            Contract.Assert(authorizationManager != null);
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetForumTopicPostsQuery query, ClaimsPrincipal principal)
        {
            return this.GetPostsQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumPostsListItemViewModel> Execute(IReadContext readContext, GetForumTopicPostsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var postsQuery = this.ApplyPagination(this.GetPostsQuery(readContext, query), pagination).ToList();
            var startIndex = (pagination != null ? pagination.PageNum * pagination.PageSize : 0) + 1;

            var posts = postsQuery.Select(
                (p, idx) => new ForumPostsListItemViewModel
                {
                    Author = new AuthorViewModel
                    {
                        Id = p.AuthorId,
                        Login = p.AuthorLogin
                    },
                    AuthorIP = p.AuthorIP,
                    CreatedDate = p.CreatedDate,
                    IsHidden = p.IsHidden,
                    Text = p.Text,
                    TopicId = p.TopicId,
                    PostId = p.PostId,
                    UpdatedDate = p.UpdatedDate,
                    IsRead = true,
                    IsFirstPost = p.IsStartPost,
                    Index = idx + startIndex,
                    VotesRating = p.VotesRating,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "ForumPost", p.PostId) && !p.IsStartPost,
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "ForumPost", p.PostId),
                    CanBeVoted = principal.IsInRole("User") && principal.GetUserId() != p.AuthorId
                })
                .ToList();

            if (principal.Identity.IsAuthenticated)
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                int[] postIdentifiers = posts.Select(p => p.PostId).ToArray();
                
                var userVotes = readContext
                    .Query<ForumPostVote>()
                    .Where(v => v.UserId == userId && postIdentifiers.Contains(v.PostId)).ToDictionary(v => v.PostId);

                var answerNotifications = this.queryProcessor
                    .Process(new GetNotificationsQuery().WithFilter<NewForumAnswerNotification>(n => n.TopicId == query.TopicId))
                    .Select(n => ((NewForumAnswerNotification)n.Data).PostId)
                    .ToArray();

                posts.ForEach(p =>
                {
                    p.IsRead = !answerNotifications.Contains(p.PostId);
                    p.UserVote = userVotes.ContainsKey(p.PostId) ? userVotes[p.PostId].Vote : (int?)null;
                });

                var firstUnread = posts.FirstOrDefault(p => !p.IsRead);

                if (firstUnread != null)
                {
                    firstUnread.FirstUnread = true;
                }
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                {
                    Authors = posts.Select(t => t.Author)
                });

            return posts;
        }

        /// <summary>
        /// Gets the posts query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The forum posts.</returns>
        private IQueryable<ForumPost> GetPostsQuery(IReadContext readContext, GetForumTopicPostsQuery query)
        {
            var queryable = readContext.Query<ForumPost>()
                .Where(p => p.TopicId == query.TopicId);

            if (!query.LoadStartPost)
            {
                queryable = queryable.Where(p => !p.IsStartPost);
            }

            return queryable.OrderBy(p => p.PostId);
        }
    }
}