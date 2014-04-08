namespace MirGames.Domain.Users.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Users.CommandHandlers;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Notifications;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Notifications;

    /// <summary>
    /// Handles the topic created event.
    /// </summary>
    internal sealed class PasswordRestoreRequestedEventListener : EventListenerBase<PasswordRestoreRequestedEvent>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The notification manager.
        /// </summary>
        private readonly INotificationManager notificationManager;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRestoreRequestedEventListener" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="notificationManager">The notification manager.</param>
        /// <param name="settings">The settings.</param>
        public PasswordRestoreRequestedEventListener(IReadContextFactory readContextFactory, INotificationManager notificationManager, ISettings settings)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(notificationManager != null);
            Contract.Requires(settings != null);
            
            this.readContextFactory = readContextFactory;
            this.notificationManager = notificationManager;
            this.settings = settings;
        }

        /// <inheritdoc />
        public override void Process(PasswordRestoreRequestedEvent @event)
        {
            User user;

            using (var readContext = this.readContextFactory.Create())
            {
                user = readContext.Query<User>().First(u => u.Id == @event.UserId);
            }

            var restoreUrl = string.Format(this.settings.GetValue<string>("RestorePassword.Url"), @event.SecretKey);

            this.notificationManager.SendNotification(
                user.Mail,
                new PasswordRestoreRequestedNotification
                    {
                        RestoreCode = @event.SecretKey,
                        RestoreLink = restoreUrl
                    });
        }
    }
}
