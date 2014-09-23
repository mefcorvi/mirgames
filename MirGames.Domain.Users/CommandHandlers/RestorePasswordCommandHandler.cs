// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RestorePasswordCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the restore password command.
    /// </summary>
    internal sealed class RestorePasswordCommandHandler : CommandHandler<RestorePasswordCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestorePasswordCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public RestorePasswordCommandHandler(IWriteContextFactory writeContextFactory, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override void Execute(RestorePasswordCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            User user;

            using (var writeContext = this.writeContextFactory.Create())
            {
                var passwordRestoreRequest =
                    writeContext.Set<PasswordRestoreRequest>().FirstOrDefault(r => r.SecretKey == command.SecretKey);

                if (passwordRestoreRequest == null)
                {
                    throw new ItemNotFoundException("PasswordRestoreRequest", command.SecretKey);
                }

                user = writeContext.Set<User>().First(r => r.Id == passwordRestoreRequest.UserId);
                user.Password = passwordRestoreRequest.NewPassword;

                writeContext.Set<PasswordRestoreRequest>().Remove(passwordRestoreRequest);

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new PasswordRestoredEvent { UserId = user.Id });
        }
    }
}
