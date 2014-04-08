namespace MirGames.Domain.Topics.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// The topic created event.
    /// </summary>
    public class TopicDeletedEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Topics.TopicRemoved"; }
        }
    }
}
