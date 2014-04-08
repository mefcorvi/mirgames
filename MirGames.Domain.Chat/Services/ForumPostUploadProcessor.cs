namespace MirGames.Domain.Chat.Services
{
    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class ChatMessageUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("chatMessage");
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
