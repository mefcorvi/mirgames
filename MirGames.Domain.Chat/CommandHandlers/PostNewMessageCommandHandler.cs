// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostNewMessageCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.CommandHandlers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Chat.Commands;
    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Post New Message command.
    /// </summary>
    internal sealed class PostNewMessageCommandHandler : CommandHandler<PostChatMessageCommand, int>
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
        /// Initializes a new instance of the <see cref="PostNewMessageCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public PostNewMessageCommandHandler(IWriteContextFactory writeContextFactory, IQueryProcessor queryProcessor, ICommandProcessor commandProcessor, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override int Execute(PostChatMessageCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int userId = principal.GetUserId().GetValueOrDefault();
            var author = this.queryProcessor.Process(
                new GetAuthorQuery
                {
                    UserId = userId
                });

            if (string.IsNullOrEmpty(command.Message))
            {
                throw new ValidationException("Message have to be specified.");
            }

            command.Message = this.commandProcessor.Execute(new TransformMentionsCommand
            {
                Text = command.Message
            });

            var message = new ChatMessage
            {
                AuthorId = userId,
                AuthorIp = principal.GetHostAddress(),
                AuthorLogin = author.Login,
                Message = command.Message,
                CreatedDate = DateTime.UtcNow
            };

            authorizationManager.EnsureAccess(principal, "Post", "ChatMessage");

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<ChatMessage>().Add(message);
                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = message.MessageId,
                            Identifiers = command.Attachments
                        });
            }

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Delete", "Edit" },
                EntityId = message.MessageId,
                EntityType = "ChatMessage",
                ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                UserId = userId
            });

            this.eventBus.Raise(
                new NewChatMessageEvent
                    {
                        AuthorId = userId,
                        Message = message.Message,
                        MessageId = message.MessageId
                    });

            return message.MessageId;
        }
    }
}
