namespace MirGames.Domain.Chat.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// The topic created event.
    /// </summary>
    public class NewChatMessageEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Chat.NewChatMessage"; }
        }
    }
}
