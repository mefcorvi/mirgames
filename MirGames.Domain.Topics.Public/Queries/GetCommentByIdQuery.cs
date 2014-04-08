namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns comments by the specified topic.
    /// </summary>
    [Api]
    public class GetCommentByIdQuery : SingleItemQuery<CommentViewModel>
    {
        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        public int CommentId { get; set; }
    }
}