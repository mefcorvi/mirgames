namespace MirGames.Domain.Topics.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Edits the comment.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api]
    public sealed class EditCommentCommand : Command
    {
        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        public int CommentId { get; set; }

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