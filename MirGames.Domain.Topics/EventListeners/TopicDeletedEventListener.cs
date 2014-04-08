namespace MirGames.Domain.Topics.EventListeners
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.SearchEngine;

    /// <summary>
    /// Handles the topic created event.
    /// </summary>
    internal sealed class TopicDeletedEventListener : EventListenerBase<TopicDeletedEvent>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicDeletedEventListener" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public TopicDeletedEventListener(ISearchEngine searchEngine, Lazy<ICommandProcessor> commandProcessor)
        {
            Contract.Requires(searchEngine != null);

            this.searchEngine = searchEngine;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(TopicDeletedEvent @event)
        {
            this.searchEngine.Remove(@event.TopicId, "Topic");
            this.commandProcessor.Value.Execute(new RemoveAttachmentsCommand { EntityId = @event.TopicId, EntityType = "topic" });
        }
    }
}
