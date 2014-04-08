namespace MirGames.Domain.Topics.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the edit comment command.
    /// </summary>
    internal sealed class EditCommentCommandHandler : CommandHandler<EditCommentCommand>
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
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        /// <param name="eventBus">The event bus.</param>
        public EditCommentCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor, ITextTransform textTransform, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(textTransform != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
            this.textTransform = textTransform;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(EditCommentCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var comment = writeContext.Set<Comment>().FirstOrDefault(t => t.CommentId == command.CommentId);
                authorizationManager.EnsureAccess(principal, "Edit", comment);

                if (comment == null)
                {
                    throw new ItemNotFoundException("Comment", command.CommentId);
                }

                comment.SourceText = command.Text;
                comment.Text = this.textTransform.Transform(command.Text);
                comment.UpdatedDate = DateTime.UtcNow;

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = command.CommentId,
                            Identifiers = command.Attachments
                        });
            }

            this.eventBus.Raise(new CommentEditedEvent { CommentId = command.CommentId });
        }
    }
}