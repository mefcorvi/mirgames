namespace MirGames.Domain.Users.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when tag removed for online user.
    /// </summary>
    public sealed class OnlineUserTagRemovedEvent : Event
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Users.OnlineUserTagRemoved"; }
        }
    }
}