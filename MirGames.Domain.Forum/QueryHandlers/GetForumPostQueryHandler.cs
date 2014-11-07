// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumPostQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumPostQueryHandler : SingleItemQueryHandler<GetForumPostQuery, ForumPostsListItemViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumPostQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetForumPostQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        protected override ForumPostsListItemViewModel Execute(IReadContext readContext, GetForumPostQuery query, ClaimsPrincipal principal)
        {
            var post = readContext.Query<ForumPost>()
                .SingleOrDefault(t => t.PostId == query.PostId);

            if (post == null)
            {
                return null;
            }

            var author = this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { new AuthorViewModel { Login = post.AuthorLogin, Id = post.AuthorId } }
                    }).Single();

            var postIndex = readContext
                .Query<ForumPost>()
                .Count(p => p.TopicId == post.TopicId && p.PostId < post.PostId);

            return new ForumPostsListItemViewModel
            {
                Author = author,
                AuthorIP = post.AuthorIP,
                CreatedDate = post.CreatedDate,
                TopicId = post.TopicId,
                UpdatedDate = post.UpdatedDate,
                IsHidden = post.IsHidden,
                PostId = post.PostId,
                Text = post.Text,
                Index = postIndex,
                IsRead = true,
                FirstUnread = false,
                CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "ForumPost", post.PostId),
                CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "ForumPost", post.PostId),
                CanBeVoted = principal.IsInRole("User") && principal.GetUserId() != post.AuthorId
            };
        }
    }
}