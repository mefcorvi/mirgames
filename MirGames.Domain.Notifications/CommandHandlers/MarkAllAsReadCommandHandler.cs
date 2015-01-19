namespace MirGames.Domain.Notifications.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    internal sealed class MarkAllAsReadCommandHandler : CommandHandler<MarkAllAsReadCommand>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkAllAsReadCommandHandler"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public MarkAllAsReadCommandHandler(ICommandProcessor commandProcessor)
        {
            Contract.Requires(commandProcessor != null);
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        protected override void Execute(MarkAllAsReadCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            this.commandProcessor.Execute(new RemoveNotificationsCommand
            {
                UserIdentifiers = new[] { principal.GetUserId().GetValueOrDefault() }
            });
        }
    }
}
