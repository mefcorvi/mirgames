namespace MirGames.Domain.Forum.Events
{
    using System;

    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    public sealed class ForumTopicRepliedEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the replied date.
        /// </summary>
        public DateTime RepliedDate { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Forum.TopicReplied"; }
        }
    }
}