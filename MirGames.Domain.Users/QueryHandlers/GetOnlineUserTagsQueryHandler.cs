// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetOnlineUserTagsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns map between user identifier and tags.
    /// </summary>
    internal sealed class GetOnlineUserTagsQueryHandler : SingleItemQueryHandler<GetOnlineUserTagsQuery, IDictionary<int, IEnumerable<string>>>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOnlineUserTagsQueryHandler"/> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public GetOnlineUserTagsQueryHandler(IOnlineUsersManager onlineUsersManager)
        {
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        protected override IDictionary<int, IEnumerable<string>> Execute(IReadContext readContext, GetOnlineUserTagsQuery query, ClaimsPrincipal principal)
        {
            return this.onlineUsersManager.GetUserTags();
        }
    }
}