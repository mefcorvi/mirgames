namespace MirGames.Domain.Forum.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The forum topic.
    /// </summary>
    internal sealed class ForumTopic
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
        /// Gets or sets the author login.
        /// </summary>
        public string AuthorLogin { get; set; }

        /// <summary>
        /// Gets or sets the author IP.
        /// </summary>
        public string AuthorIp { get; set; }

        /// <summary>
        /// Gets or sets the last post author unique identifier.
        /// </summary>
        public int? LastPostAuthorId { get; set; }

        /// <summary>
        /// Gets or sets the last post author login.
        /// </summary>
        public string LastPostAuthorLogin { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the posts count.
        /// </summary>
        public int PostsCount { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        public ICollection<ForumPost> Posts { get; set; }
    }
}