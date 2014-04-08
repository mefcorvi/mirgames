namespace MirGames.Domain.Users.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event raised whether user avatar has been changed.
    /// </summary>
    public class UserAvatarChangedEvent : Event
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Users.UserAvatarChanged"; }
        }
    }
}