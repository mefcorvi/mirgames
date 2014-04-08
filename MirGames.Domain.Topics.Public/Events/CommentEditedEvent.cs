namespace MirGames.Domain.Topics.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// The comment deleted event.
    /// </summary>
    public class CommentEditedEvent : Event
    {
        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        public int CommentId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Topics.CommentEdited"; }
        }
    }
}