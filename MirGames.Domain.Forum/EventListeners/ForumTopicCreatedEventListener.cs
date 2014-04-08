namespace MirGames.Domain.Forum.EventListeners
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about new topic created.
    /// </summary>
    internal sealed class ForumTopicCreatedEventListener : EventListenerBase<ForumTopicCreatedEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicCreatedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumTopicCreatedEventListener(Lazy<ICommandProcessor> commandProcessor)
        {
            Contract.Requires(commandProcessor != null);

            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicCreatedEvent @event)
        {
            Contract.Requires(@event != null);

            this.commandProcessor.Value.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
            this.commandProcessor.Value.Execute(new MarkTopicAsUnreadForUsersCommand { TopicId = @event.TopicId, TopicDate = @event.CreationDate });
            this.commandProcessor.Value.Execute(new MarkTopicAsReadCommand { TopicId = @event.TopicId });
        }
    }
}