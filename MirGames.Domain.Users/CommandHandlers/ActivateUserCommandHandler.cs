// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ActivateUserCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
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
    internal sealed class ActivateUserCommandHandler : CommandHandler<ActivateUserCommand, string>
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
        /// Initializes a new instance of the <see cref="ActivateUserCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        public ActivateUserCommandHandler(IWriteContextFactory writeContextFactory, IEventLog eventLog)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);

            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        public override string Execute(ActivateUserCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(command.ActivationKey));
            var sessionId = Guid.NewGuid().ToString("N");
            User user;

            using (var writeContext = this.writeContextFactory.Create())
            {
                user = writeContext.Set<User>().SingleOrDefault(u => u.UserActivationKey == command.ActivationKey);

                if (user == null)
                {
                    return null;
                }

                user.UserActivationKey = null;
                user.IsActivated = true;

                var oldSession = writeContext.Set<UserSession>().SingleOrDefault(u => u.UserId == user.Id);

                if (oldSession != null)
                {
                    writeContext.Set<UserSession>().Remove(oldSession);
                }

                writeContext.Set<UserSession>().Add(
                    new UserSession
                        {
                            CreateDate = oldSession != null ? oldSession.CreateDate : DateTime.UtcNow,
                            LastDate = DateTime.UtcNow,
                            CreationIP = oldSession != null ? oldSession.CreationIP : principal.GetHostAddress(),
                            LastVisitIP = principal.GetHostAddress(),
                            UserId = user.Id,
                            Id = sessionId
                        });

                writeContext.SaveChanges();
            }

            this.eventLog.LogInformation("ActivateUserCommandHandler", "User \"{0}\" activated and singed-in with session \"{1}\"", user.Login, sessionId);

            return sessionId;
        }
    }
}