namespace MirGames.Domain.Forum.EventListeners
{
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumPostDeletedEventListener : EventListenerBase<ForumPostDeletedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostDeletedEventListener"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumPostDeletedEventListener(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumPostDeletedEvent @event)
        {
            this.commandProcessor.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
            this.commandProcessor.Execute(new RemoveAttachmentsCommand { EntityId = @event.PostId, EntityType = "forumPost" });
        }
    }
}