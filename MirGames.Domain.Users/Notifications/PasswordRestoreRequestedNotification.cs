namespace MirGames.Domain.Users.Notifications
{
    using MirGames.Domain.Users;
    using MirGames.Infrastructure.Notifications;

    /// <summary>
    /// The password restore requested.
    /// </summary>
    public sealed class PasswordRestoreRequestedNotification : Notification
    {
        /// <inheritdoc />
        public override string Title
        {
            get { return Localization.PasswordRestoreRequestedNotification_Title; }
        }

        /// <inheritdoc />
        public override string Body
        {
            get { return Localization.PasswordRestoreRequestedNotification_Body; }
        }

        /// <summary>
        /// Gets or sets the restore code.
        /// </summary>
        public string RestoreCode { get; set; }

        /// <summary>
        /// Gets or sets the restore link.
        /// </summary>
        public string RestoreLink { get; set; }
    }
}
