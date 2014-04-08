namespace MirGames.Domain.Attachments.Entities
{
    using System;

    /// <summary>
    /// The attachment entity.
    /// </summary>
    internal sealed class Attachment
    {
        /// <summary>
        /// Gets or sets the attachment unique identifier.
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the type of the attachment.
        /// </summary>
        public string AttachmentType { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Gets or sets the entity unique identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether file is published.
        /// </summary>
        public bool IsPublished { get; set; }
    }
}
