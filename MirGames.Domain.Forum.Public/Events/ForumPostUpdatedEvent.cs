namespace MirGames.Domain.Forum.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic updated.
    /// </summary>
    public sealed class ForumPostUpdatedEvent : Event
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
            get { return "Forum.ForumPostUpdated"; }
        }
    }
}