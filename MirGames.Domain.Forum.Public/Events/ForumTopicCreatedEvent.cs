namespace MirGames.Domain.Forum.Events
{
    using System;

    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about new topic created.
    /// </summary>
    public sealed class ForumTopicCreatedEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Forum.TopicCreated"; }
        }
    }
}