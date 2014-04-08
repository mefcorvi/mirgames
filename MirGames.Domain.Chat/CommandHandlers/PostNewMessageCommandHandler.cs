namespace MirGames.Domain.Chat.CommandHandlers
{
    using System;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Chat.Commands;
    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Security;
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
        public override int Execute(PostChatMessageCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int userId = principal.GetUserId().GetValueOrDefault();
            var author = this.queryProcessor.Process(
                new GetAuthorQuery
                {
                    UserId = userId
                });

            var message = new ChatMessage
            {
                AuthorId = userId,
                AuthorIp = principal.GetHostAddress(),
                AuthorLogin = author.Login,
                Message = command.Message,
                CreatedDate = DateTime.UtcNow
            };

            authorizationManager.EnsureAccess(principal, "Post", message);

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
