namespace MirGames.Domain.Forum.EventListeners
{
    using System;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumTopicRepliedEventListener : EventListenerBase<ForumTopicRepliedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicRepliedEventListener"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumTopicRepliedEventListener(Lazy<ICommandProcessor> commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicRepliedEvent @event)
        {
            this.commandProcessor.Value.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
            this.commandProcessor.Value.Execute(new MarkTopicAsUnreadForUsersCommand { TopicId = @event.TopicId, TopicDate = @event.RepliedDate });
            this.commandProcessor.Value.Execute(new MarkTopicAsReadCommand { TopicId = @event.TopicId });
        }
    }
}