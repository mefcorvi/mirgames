// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="OAuthLoginCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the OAuth login command.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed class OAuthLoginCommandHandler : CommandHandler<OAuthLoginCommand, string>
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
        /// Initializes a new instance of the <see cref="OAuthLoginCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        public OAuthLoginCommandHandler(IWriteContextFactory writeContextFactory, IEventLog eventLog)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);

            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        protected override string Execute(OAuthLoginCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            var sessionId = Guid.NewGuid().ToString("N");

            using (var writeContext = this.writeContextFactory.Create())
            {
                string providerName = command.ProviderName.ToLower();

                var provider = writeContext.Set<OAuthProvider>().FirstOrDefault(p => p.Name == providerName);

                if (provider == null)
                {
                    throw new ItemNotFoundException("OAuth Provider", providerName);
                }

                var authToken = writeContext.Set<OAuthToken>().FirstOrDefault(
                    t => t.ProviderUserId == command.ProviderUserId && t.ProviderId == provider.Id);

                if (authToken == null)
                {
                    return null;
                }

                var user = writeContext.Set<User>().First(u => u.Id == authToken.UserId);

                writeContext.Set<UserSession>().Add(
                    new UserSession
                        {
                            CreateDate = DateTime.UtcNow,
                            LastDate = DateTime.UtcNow,
                            CreationIP = principal.GetHostAddress(),
                            LastVisitIP = principal.GetHostAddress(),
                            UserId = user.Id,
                            Id = sessionId
                        });

                writeContext.SaveChanges();
            }

            this.eventLog.LogInformation(
                "OAuthLoginCommandHandler",
                "User \"{0}\" singed-in using \"{1}\" authentication provider",
                command.ProviderName,
                sessionId);

            return sessionId;
        }
    }
}