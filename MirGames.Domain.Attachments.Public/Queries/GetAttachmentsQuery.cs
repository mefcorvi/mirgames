namespace MirGames.Domain.Attachments.Queries
{
    using MirGames.Domain.Attachments.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the attachments.
    /// </summary>
    public class GetAttachmentsQuery : Query<AttachmentViewModel>
    {
        /// <summary>
        /// Gets or sets the entity unique identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attachment is image.
        /// </summary>
        public bool? IsImage { get; set; }
    }
}