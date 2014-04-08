namespace MirGames.Domain.Forum.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic deleted.
    /// </summary>
    public sealed class ForumTopicDeletedEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Forum.ForumTopicDeleted"; }
        }
    }
}