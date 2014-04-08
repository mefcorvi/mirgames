namespace MirGames.Domain.Chat.Queries
{
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the chat messages.
    /// </summary>
    [Api]
    public sealed class GetChatMessagesQuery : Query<ChatMessageViewModel>
    {
        /// <summary>
        /// Gets or sets the maximum identifier of received messages.
        /// </summary>
        public int? LastIndex { get; set; }

        /// <summary>
        /// Gets or sets the minimum identifier of received messages.
        /// </summary>
        public int? FirstIndex { get; set; }
    }
}
