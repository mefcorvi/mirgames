namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Notifications;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Notifications;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The resend activation command handler.
    /// </summary>
    internal sealed class ResendActivationCommandHandler : CommandHandler<ResendActivationCommand>
    {
        /// <summary>
        /// The write context factory.
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
        /// Initializes a new instance of the <see cref="ResendActivationCommandHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="notificationManager">The notification manager.</param>
        /// <param name="settings">The settings.</param>
        public ResendActivationCommandHandler(IReadContextFactory readContextFactory, INotificationManager notificationManager, ISettings settings)
        {
            this.readContextFactory = readContextFactory;
            this.notificationManager = notificationManager;
            this.settings = settings;
        }

        /// <inheritdoc />
        public override void Execute(ResendActivationCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var userId = principal.GetUserId().GetValueOrDefault();
            User user;

            using (var readContext = this.readContextFactory.Create())
            {
                user = readContext.Query<User>().First(u => u.Id == userId);
            }
            
            var activationUrl = string.Format(this.settings.GetValue<string>("Activation.Url"), user.UserActivationKey);

            this.notificationManager.SendNotification(
                user.Mail,
                new ActivationRequestedNotification
                    {
                        ActivationCode = user.UserActivationKey,
                        ActivationLink = activationUrl
                    });
        }
    }
}