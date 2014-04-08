namespace MirGames.Domain.Topics.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The comment entity.
    /// </summary>
    internal sealed class Comment
    {
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user login.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the source text.
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user IP.
        /// </summary>
        public string UserIP { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        public Topic Topic { get; set; }
    }
}
