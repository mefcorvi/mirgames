namespace MirGames.Domain.Users.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when user has successfully restored his password.
    /// </summary>
    public sealed class PasswordRestoredEvent : Event
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "User.PasswordRestoredEvent"; }
        }
    }
}