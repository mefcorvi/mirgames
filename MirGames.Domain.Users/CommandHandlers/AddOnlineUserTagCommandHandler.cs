namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Adds tag for the online user.
    /// </summary>
    internal sealed class AddOnlineUserTagCommandHandler : CommandHandler<AddOnlineUserTagCommand>
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
        /// Initializes a new instance of the <see cref="AddOnlineUserTagCommandHandler" /> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        /// <param name="eventBus">The event bus.</param>
        public AddOnlineUserTagCommandHandler(IOnlineUsersManager onlineUsersManager, IEventBus eventBus)
        {
            this.onlineUsersManager = onlineUsersManager;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(AddOnlineUserTagCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var expirationTime = command.ExpirationTime.HasValue
                                     ? TimeSpan.FromMilliseconds(command.ExpirationTime.Value)
                                     : (TimeSpan?)null;

            this.onlineUsersManager.AddUserTag(principal, command.Tag, expirationTime);
            this.eventBus.Raise(new OnlineUserTagAddedEvent { Tag = command.Tag, UserId = principal.GetUserId().GetValueOrDefault() });
        }
    }
}
