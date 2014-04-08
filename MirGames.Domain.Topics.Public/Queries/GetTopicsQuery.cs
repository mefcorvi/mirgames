namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topics that are should be shown on the main page.
    /// </summary>
    public sealed class GetTopicsQuery : Query<TopicsListItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsQuery"/> class.
        /// </summary>
        public GetTopicsQuery()
        {
            this.IsPublished = true;
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only published topics should be return.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }
    }
}
