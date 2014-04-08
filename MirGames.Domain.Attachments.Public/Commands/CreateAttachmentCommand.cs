namespace MirGames.Domain.Attachments.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Marks the specified topic as read.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class CreateAttachmentCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }
    }
}