namespace MirGames.Domain.Topics.EventListeners
{
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.SearchEngine;

    /// <summary>
    /// Handles the topic created event.
    /// </summary>
    internal sealed class TopicCreatedEventListener : EventListenerBase<TopicCreatedEvent>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicCreatedEventListener"/> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        public TopicCreatedEventListener(ISearchEngine searchEngine)
        {
            Contract.Requires(searchEngine != null);

            this.searchEngine = searchEngine;
        }

        /// <inheritdoc />
        public override void Process(TopicCreatedEvent @event)
        {
            this.searchEngine.Index(@event.TopicId, "Topic", @event.Title + " " + @event.Text + " " + @event.Tags);
        }
    }
}
