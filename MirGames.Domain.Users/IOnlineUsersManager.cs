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
