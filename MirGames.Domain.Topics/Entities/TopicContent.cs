namespace MirGames.Domain.Topics.Entities
{
    /// <summary>
    /// The topic content entity.
    /// </summary>
    internal sealed class TopicContent
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic text.
        /// </summary>
        public string TopicText { get; set; }

        /// <summary>
        /// Gets or sets the topic text short.
        /// </summary>
        public string TopicTextShort { get; set; }

        /// <summary>
        /// Gets or sets the topic text source.
        /// </summary>
        public string TopicTextSource { get; set; }

        /// <summary>
        /// Gets or sets the topic extra.
        /// </summary>
        public string TopicExtra { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        public Topic Topic { get; set; }
    }
}
