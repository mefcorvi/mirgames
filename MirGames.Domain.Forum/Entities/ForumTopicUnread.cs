namespace MirGames.Domain.Forum.Entities
{
    using System;

    /// <summary>
    /// The forum topic unread.
    /// </summary>
    internal sealed class ForumTopicUnread
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unread date.
        /// </summary>
        public DateTime UnreadDate { get; set; }
    }
}