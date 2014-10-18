// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetBlogByEntityQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    using ClaimsPrincipalExtensions = MirGames.Domain.Security.ClaimsPrincipalExtensions;

    internal sealed class GetBlogByEntityQueryHandler : SingleItemQueryHandler<GetBlogByEntityQuery, BlogViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogByEntityQueryHandler"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetBlogByEntityQueryHandler(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        protected override BlogViewModel Execute(IReadContext readContext, GetBlogByEntityQuery query, ClaimsPrincipal principal)
        {
            var blog = readContext
                .Query<Blog>()
                .FirstOrDefault(b => b.EntityId == query.EntityId && b.EntityType == query.EntityType);

            if (blog == null)
            {
                return null;
            }

            int userId = ClaimsPrincipalExtensions.GetUserId(principal).GetValueOrDefault();

            return new BlogViewModel
            {
                BlogId = blog.Id,
                Description = blog.Description,
                EntityId = blog.EntityId,
                EntityType = blog.EntityType,
                Title = blog.Title,
                CanAddTopic = this.authorizationManager.CheckAccess(userId, "CreateTopic", "Blog", blog.Id)
            };
        }
    }
}
