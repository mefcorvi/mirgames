namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Removes tag for the online user.
    /// </summary>
    internal sealed class RemoveOnlineUserTagCommandHandler : CommandHandler<RemoveOnlineUserTagCommand>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveOnlineUserTagCommandHandler" /> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        /// <param name="eventBus">The event bus.</param>
        public RemoveOnlineUserTagCommandHandler(IOnlineUsersManager onlineUsersManager, IEventBus eventBus)
        {
            this.onlineUsersManager = onlineUsersManager;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(RemoveOnlineUserTagCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            this.onlineUsersManager.RemoveUserTag(principal, command.Tag);
            this.eventBus.Raise(new OnlineUserTagRemovedEvent { Tag = command.Tag, UserId = principal.GetUserId().GetValueOrDefault() });
        }
    }
}
