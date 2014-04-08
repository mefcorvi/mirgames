namespace MirGames.Domain.Topics.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Deletes the comment.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api]
    public class DeleteCommentCommand : Command
    {
        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        /// <value>
        /// The comment unique identifier.
        /// </value>
        public int CommentId { get; set; }
    }
}