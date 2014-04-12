// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumPostForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumPostForEditQueryHandler : SingleItemQueryHandler<GetForumPostForEditQuery, ForumPostForEditViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumPostForEditQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetForumPostForEditQueryHandler(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override ForumPostForEditViewModel Execute(IReadContext readContext, GetForumPostForEditQuery query, ClaimsPrincipal principal)
        {
            var post = readContext.Query<ForumPost>()
                .Include(p => p.Topic)
                .FirstOrDefault(p => p.PostId == query.PostId);

            if (post == null)
            {
                return null;
            }

            this.authorizationManager.EnsureAccess(principal, "Edit", post);

            return new ForumPostForEditViewModel
            {
                PostId = post.PostId,
                SourceText = post.SourceText,
                TopicTags = post.Topic.TagsList,
                TopicTitle = post.Topic.Title,
                CanChangeTags = post.IsStartPost && this.authorizationManager.CheckAccess(principal, "Edit", post.Topic),
                CanChangeTitle = post.IsStartPost && this.authorizationManager.CheckAccess(principal, "Edit", post.Topic),
            };
        }
    }
}