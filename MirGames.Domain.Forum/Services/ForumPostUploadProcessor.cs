namespace MirGames.Domain.Forum.Services
{
    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class ForumPostUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("forumPost");
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
