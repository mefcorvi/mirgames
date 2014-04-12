// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetOnlineUsersQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class GetOnlineUsersQueryHandler : QueryHandler<GetOnlineUsersQuery, OnlineUserViewModel>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOnlineUsersQueryHandler" /> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public GetOnlineUsersQueryHandler(IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(onlineUsersManager != null);

            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetOnlineUsersQuery query, ClaimsPrincipal principal)
        {
            return this.GetOnlineUsers().Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<OnlineUserViewModel> Execute(IReadContext readContext, GetOnlineUsersQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.GetOnlineUsers().OrderBy(u => u.LastRequestDate);
        }

        /// <summary>
        /// Gets the online users.
        /// </summary>
        /// <returns>Sequence of online users.</returns>
        private IEnumerable<OnlineUserViewModel> GetOnlineUsers()
        {
            return this.onlineUsersManager.GetOnlineUsers();
        }
    }
}