namespace MirGames.Domain.Users.CommandHandlers
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when password restore is requested.
    /// </summary>
    public sealed class PasswordRestoreRequestedEvent : Event
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public string SecretKey { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "User.PasswordRestoreRequested"; }
        }
    }
}