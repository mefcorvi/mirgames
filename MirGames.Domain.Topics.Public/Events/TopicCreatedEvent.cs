namespace MirGames.Domain.Topics.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// The topic created event.
    /// </summary>
    public class TopicCreatedEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Topics.TopicCreated"; }
        }
    }
}
