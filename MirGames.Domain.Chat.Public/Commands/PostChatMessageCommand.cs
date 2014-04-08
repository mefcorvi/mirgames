namespace MirGames.Domain.Chat.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The message command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api]
    public sealed class PostChatMessageCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IEnumerable<int> Attachments { get; set; }
    }
}
