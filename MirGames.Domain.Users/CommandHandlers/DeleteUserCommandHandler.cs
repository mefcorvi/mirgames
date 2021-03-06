// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DeleteUserCommandHandler.cs">
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

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class DeleteUserCommandHandler : CommandHandler<DeleteUserCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public DeleteUserCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override void Execute(DeleteUserCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var user = writeContext.Set<User>().SingleOrDefault(t => t.Id == command.UserId);

                if (user == null)
                {
                    throw new ItemNotFoundException("User", command.UserId);
                }

                authorizationManager.EnsureAccess(principal, "Delete", "User", user.Id);

                var userSessions = writeContext.Set<UserSession>().Where(us => us.UserId == user.Id);
                writeContext.Set<UserSession>().RemoveRange(userSessions);

                writeContext.Set<User>().Remove(user);
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new RemovePermissionsCommand
            {
                UserId = command.UserId
            });

            this.eventBus.Raise(new UserDeletedEvent
            {
                UserId = command.UserId
            });
        }
    }
}