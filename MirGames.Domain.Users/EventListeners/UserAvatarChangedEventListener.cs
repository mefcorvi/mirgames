// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserAvatarChangedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Handles the avatar changed event.
    /// </summary>
    internal sealed class UserAvatarChangedEventListener : EventListenerBase<UserAvatarChangedEvent>
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAvatarChangedEventListener" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public UserAvatarChangedEventListener(ICacheManagerFactory cacheManagerFactory, IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(cacheManagerFactory != null);
            Contract.Requires(onlineUsersManager != null);

            this.cacheManager = cacheManagerFactory.Create("Users");
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        public override void Process(UserAvatarChangedEvent @event)
        {
            this.cacheManager.Remove("GetAuthorsQueryHandler.User" + @event.UserId);
            var cachedUser = this.onlineUsersManager.GetOnlineUsers().FirstOrDefault(user => user.Id == @event.UserId);

            if (cachedUser != null)
            {
                cachedUser.AvatarUrl = @event.AvatarUrl;
            }
        }
    }
}
