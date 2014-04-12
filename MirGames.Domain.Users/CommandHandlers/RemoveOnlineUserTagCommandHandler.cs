// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RemoveOnlineUserTagCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
