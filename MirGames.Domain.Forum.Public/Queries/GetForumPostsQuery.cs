namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns list of posts.
    /// </summary>
    public class GetForumPostsQuery : Query<ForumPostViewModel>
    {
        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int? AuthorId { get; set; }
    }
}