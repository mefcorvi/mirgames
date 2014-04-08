namespace MirGames.Domain.Topics.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Adds new comment to the topic and returns an identifier of the command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api]
    public class PostNewCommentCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IEnumerable<int> Attachments { get; set; }
    }
}
