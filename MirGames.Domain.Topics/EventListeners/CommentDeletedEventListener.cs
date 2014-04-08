namespace MirGames.Domain.Topics.EventListeners
{
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about deleted comment
    /// </summary>
    internal sealed class CommentDeletedEventListener : EventListenerBase<CommentDeletedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDeletedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public CommentDeletedEventListener(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(CommentDeletedEvent @event)
        {
            this.commandProcessor.Execute(new RemoveAttachmentsCommand { EntityId = @event.CommentId, EntityType = "comment" });
        }
    }
}