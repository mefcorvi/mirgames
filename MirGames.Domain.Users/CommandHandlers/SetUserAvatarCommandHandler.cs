// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SetUserAvatarCommandHandler.cs">
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
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Queries;
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
    internal sealed class SetUserAvatarCommandHandler : CommandHandler<SetUserAvatarCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetUserAvatarCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public SetUserAvatarCommandHandler(IWriteContextFactory writeContextFactory, IQueryProcessor queryProcessor, ICommandProcessor commandProcessor, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(SetUserAvatarCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int userId = principal.GetUserId().GetValueOrDefault();
            var attachment = this.queryProcessor.Process(
                new GetAttachmentInfoQuery
                    {
                        AttachmentId = command.AvatarAttachmentId
                    });

            using (var writeContext = this.writeContextFactory.Create())
            {
                var user = writeContext.Set<User>().SingleOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    throw new ItemNotFoundException("User", userId);
                }

                user.AvatarUrl = attachment.AttachmentUrl;
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(
                new PublishAttachmentsCommand
                    {
                        EntityId = userId,
                        Identifiers = new[] { attachment.AttachmentId }
                    });

            this.eventBus.Raise(
                new UserAvatarChangedEvent
                    {
                        AvatarUrl = attachment.AttachmentUrl,
                        UserId = userId
                    });
        }
    }
}