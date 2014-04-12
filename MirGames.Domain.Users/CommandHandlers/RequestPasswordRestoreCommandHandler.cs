// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RequestPasswordRestoreCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the requests for password restoring.
    /// </summary>
    internal sealed class RequestPasswordRestoreCommandHandler : CommandHandler<RequestPasswordRestoreCommand>
    {
        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The password hash provider.
        /// </summary>
        private readonly IPasswordHashProvider passwordHashProvider;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPasswordRestoreCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="passwordHashProvider">The password hash provider.</param>
        /// <param name="eventBus">The event bus.</param>
        public RequestPasswordRestoreCommandHandler(IWriteContextFactory writeContextFactory, IPasswordHashProvider passwordHashProvider, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.passwordHashProvider = passwordHashProvider;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(RequestPasswordRestoreCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            User user;
            string secretKey = Guid.NewGuid().ToString().GetMd5Hash();

            using (var writeContext = this.writeContextFactory.Create())
            {
                user = writeContext.Set<User>().SingleOrDefault(u => u.Mail == command.EmailOrLogin || u.Login == command.EmailOrLogin);

                if (user == null)
                {
                    throw new ItemNotFoundException("User", command.EmailOrLogin);
                }

                var passwordRestoreRequest = new PasswordRestoreRequest
                    {
                        CreationDate = DateTime.UtcNow,
                        NewPassword = this.passwordHashProvider.GetHash(command.NewPasswordHash, user.PasswordSalt),
                        SecretKey = secretKey,
                        UserId = user.Id
                    };

                var oldRequets = writeContext.Set<PasswordRestoreRequest>().Where(r => r.UserId == user.Id);
                writeContext.Set<PasswordRestoreRequest>().RemoveRange(oldRequets);
                writeContext.Set<PasswordRestoreRequest>().Add(passwordRestoreRequest);

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new PasswordRestoreRequestedEvent { UserId = user.Id, SecretKey = secretKey });
        }
    }
}