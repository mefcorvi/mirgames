namespace MirGames.Domain.Forum.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    public sealed class ForumPostDeletedEvent : Event
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Forum.ForumPostDeleted"; }
        }
    }
}