namespace MirGames.Domain.Chat.ViewModels
{
    /// <summary>
    /// The chat message view model.
    /// </summary>
    public sealed class ChatMessageForEditViewModel
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string SourceText { get; set; }
    }
}
