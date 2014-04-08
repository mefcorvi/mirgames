namespace MirGames.Domain.Attachments.Queries
{
    using MirGames.Domain.Attachments.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the attachment info.
    /// </summary>
    public class GetAttachmentInfoQuery : SingleItemQuery<AttachmentViewModel>
    {
        /// <summary>
        /// Gets or sets the attachment unique identifier.
        /// </summary>
        public int AttachmentId { get; set; }
    }
}