namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns list of posts.
    /// </summary>
    [Api]
    public class GetForumTopicPostsQuery : Query<ForumPostsListItemViewModel>
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether first post should be loaded.
        /// </summary>
        public bool LoadStartPost { get; set; }
    }
}