// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="LoginAsUserCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class LoginAsUserCommandHandler : CommandHandler<LoginAsUserCommand, string>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginAsUserCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        public LoginAsUserCommandHandler(IWriteContextFactory writeContextFactory, IEventLog eventLog)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);

            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        protected override string Execute(LoginAsUserCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(principal.GetSessionId() != null);

            var sessionId = Guid.NewGuid().ToString("N");
            User targetUser;

            using (var writeContext = this.writeContextFactory.Create())
            {
                targetUser = writeContext.Set<User>().SingleOrDefault(u => u.Id == command.UserId);

                if (targetUser == null)
                {
                    return null;
                }

                authorizationManager.EnsureAccess(principal, "SwitchUser", "User", targetUser.Id);

                string oldSesionId = principal.GetSessionId();
                var oldSession = writeContext.Set<UserSession>().FirstOrDefault(s => s.Id == oldSesionId);
                writeContext.Set<UserSession>().Remove(oldSession);
                
                writeContext.Set<UserSession>().Add(
                    new UserSession
                        {
                            CreateDate = DateTime.UtcNow,
                            LastDate = DateTime.UtcNow,
                            CreationIP = principal.GetHostAddress(),
                            LastVisitIP = principal.GetHostAddress(),
                            UserId = targetUser.Id,
                            Id = sessionId
                        });

                writeContext.SaveChanges();
            }

            this.eventLog.LogInformation("LoginAsUserCommandHandler", "Switching user to the \"{0}\" with session \"{1}\"", targetUser.Login, sessionId);

            return sessionId;
        }
    }
}