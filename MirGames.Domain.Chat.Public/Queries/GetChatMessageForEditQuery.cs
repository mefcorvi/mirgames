namespace MirGames.Domain.Chat.Queries
{
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the chat messages.
    /// </summary>
    [Api]
    public sealed class GetChatMessageForEditQuery : SingleItemQuery<ChatMessageForEditViewModel>
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        public int MessageId { get; set; }
    }
}
