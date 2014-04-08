namespace MirGames.Domain.Chat.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Chat.Commands;
    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Post New Message command.
    /// </summary>
    internal sealed class UpdateNewMessageCommandHandler : CommandHandler<UpdateChatMessageCommand>
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
        /// Initializes a new instance of the <see cref="UpdateNewMessageCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public UpdateNewMessageCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(UpdateChatMessageCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int userId = principal.GetUserId().GetValueOrDefault();

            ChatMessage message;

            using (var writeContext = this.writeContextFactory.Create())
            {
                message = writeContext.Set<ChatMessage>().First(m => m.MessageId == command.MessageId);

                if (message == null)
                {
                    throw new ItemNotFoundException("ChatMessage", command.MessageId);
                }

                authorizationManager.EnsureAccess(principal, "Edit", message);

                message.Message = command.Message;
                message.UpdatedDate = DateTime.UtcNow;
                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = message.AuthorId.GetValueOrDefault(),
                            Identifiers = command.Attachments
                        });
            }

            this.eventBus.Raise(
                new ChatMessageUpdatedEvent
                    {
                        AuthorId = userId,
                        Message = message.Message,
                        MessageId = message.MessageId
                    });
        }
    }
}
