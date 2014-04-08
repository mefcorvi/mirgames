namespace MirGames.Domain.Attachments.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Publishes the attachment.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class RemoveAttachmentsCommand : Command
    {
        /// <summary>
        /// Gets or sets the entity unique identifier.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }
    }
}