// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="LogoutCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The logout command handler.
    /// </summary>
    internal sealed class LogoutCommandHandler : CommandHandler<LogoutCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public LogoutCommandHandler(IWriteContextFactory writeContextFactory, IOnlineUsersManager onlineUsersManager)
        {
            this.writeContextFactory = writeContextFactory;
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        protected override void Execute(LogoutCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(principal.GetSessionId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                string sessionId = principal.GetSessionId();

                var oldSession = writeContext.Set<UserSession>().SingleOrDefault(s => s.UserId == userId && s.Id == sessionId);

                if (oldSession != null)
                {
                    writeContext.Set<UserSession>().Remove(oldSession);
                }

                writeContext.SaveChanges();
            }

            this.onlineUsersManager.MarkUserAsOffline(principal);
        }
    }
}