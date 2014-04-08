namespace MirGames.Domain.Users.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event raised whether user goes offline.
    /// </summary>
    public class UserOfflineEvent : Event
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Users.UserOffline"; }
        }
    }
}