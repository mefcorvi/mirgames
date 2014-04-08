namespace MirGames.Domain.Forum.Entities
{
    /// <summary>
    /// The forum topic unread.
    /// </summary>
    internal sealed class ForumTopicRead
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int StartTopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int EndTopicId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int? UserId { get; set; }
    }
}