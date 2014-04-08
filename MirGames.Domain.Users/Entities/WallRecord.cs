namespace MirGames.Domain.Users.Entities
{
    using System;

    /// <summary>
    /// The wall record.
    /// </summary>
    internal class WallRecord
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the wall user unique identifier.
        /// </summary>
        public int WallUserId { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the replies count.
        /// </summary>
        public int RepliesCount { get; set; }

        /// <summary>
        /// Gets or sets the last reply unique identifier.
        /// </summary>
        public string LastReplyId { get; set; }

        /// <summary>
        /// Gets or sets the date add.
        /// </summary>
        public DateTime DateAdd { get; set; }

        /// <summary>
        /// Gets or sets the author IP.
        /// </summary>
        public string AuthorIp { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the wall user.
        /// </summary>
        public User WallUser { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public User Author { get; set; }
    }
}
