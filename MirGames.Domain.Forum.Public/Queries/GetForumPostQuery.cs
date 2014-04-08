namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns post by identifier.
    /// </summary>
    [Api]
    public sealed class GetForumPostQuery : SingleItemQuery<ForumPostsListItemViewModel>
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }
    }
}