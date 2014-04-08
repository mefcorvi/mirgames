namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns comments for edit.
    /// </summary>
    [Api]
    public class GetCommentForEditQuery : SingleItemQuery<CommentForEditViewModel>
    {
        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        public int CommentId { get; set; }
    }
}