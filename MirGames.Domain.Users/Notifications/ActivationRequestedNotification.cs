namespace MirGames.Domain.Users.Notifications
{
    using MirGames.Domain.Users;
    using MirGames.Infrastructure.Notifications;

    /// <summary>
    /// The account activation message.
    /// </summary>
    public sealed class ActivationRequestedNotification : Notification
    {
        /// <inheritdoc />
        public override string Title
        {
            get { return Localization.ActivationRequestedNotification_Title; }
        }

        /// <inheritdoc />
        public override string Body
        {
            get { return Localization.ActivationRequestedNotification_Body; }
        }

        /// <summary>
        /// Gets or sets the activation code.
        /// </summary>
        public string ActivationCode { get; set; }

        /// <summary>
        /// Gets or sets the activation link.
        /// </summary>
        public string ActivationLink { get; set; }
    }
}
