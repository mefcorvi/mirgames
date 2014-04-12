// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IOnlineUsersManager.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// Manages the online users.
    /// </summary>
    internal interface IOnlineUsersManager
    {
        /// <summary>
        /// Marks current user as online.
        /// </summary>
        /// <param name="principal">The principal.</param>
        void Ping(ClaimsPrincipal principal);

        /// <summary>
        /// Marks the user as offline.
        /// </summary>
        /// <param name="principal">The principal.</param>
        void MarkUserAsOffline(ClaimsPrincipal principal);

        /// <summary>
        /// Gets the online users.
        /// </summary>
        /// <returns>The online users.</returns>
        IEnumerable<OnlineUserViewModel> GetOnlineUsers();

        /// <summary>
        /// Adds the user tag.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="expirationTime">The expiration time.</param>
        void AddUserTag(ClaimsPrincipal principal, string tag, TimeSpan? expirationTime);

        /// <summary>
        /// Removes the user tag.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="tag">The tag.</param>
        void RemoveUserTag(ClaimsPrincipal principal, string tag);

        /// <summary>
        /// Gets the user tags.
        /// </summary>
        /// <returns>Map between user identifier and tags.</returns>
        IDictionary<int, IEnumerable<string>> GetUserTags();
    }
}
