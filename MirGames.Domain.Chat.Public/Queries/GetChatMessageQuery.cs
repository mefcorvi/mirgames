namespace MirGames.Domain.Chat.Queries
{
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the chat messages.
    /// </summary>
    public sealed class GetChatMessageQuery : SingleItemQuery<ChatMessageViewModel>
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        public int MessageId { get; set; }
    }
}
