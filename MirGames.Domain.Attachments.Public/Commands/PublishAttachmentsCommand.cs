namespace MirGames.Domain.Attachments.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Publishes the attachment.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class PublishAttachmentsCommand : Command
    {
        /// <summary>
        /// Gets or sets the attachment unique identifier.
        /// </summary>
        public IEnumerable<int> Identifiers { get; set; }

        /// <summary>
        /// Gets or sets the entity unique identifier.
        /// </summary>
        public int EntityId { get; set; }
    }
}