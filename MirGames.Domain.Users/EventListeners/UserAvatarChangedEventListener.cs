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
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public UserAvatarChangedEventListener(ICacheManager cacheManager, IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(cacheManager != null);

            this.cacheManager = cacheManager;
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
