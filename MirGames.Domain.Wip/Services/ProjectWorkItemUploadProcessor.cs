namespace MirGames.Domain.Wip.Services
{
    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class ProjectWorkItemUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("project-work-item");
        }

        /// <inheritdoc />
        public bool IsValid(string filePath)
        {
            return true;
        }

        /// <inheritdoc />
        public void Process(string filePath)
        {
        }
    }
}