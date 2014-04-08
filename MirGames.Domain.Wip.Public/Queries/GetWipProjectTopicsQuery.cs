namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topics that are should be shown on the main page.
    /// </summary>
    public sealed class GetWipProjectTopicsQuery : Query<TopicsListItem>
    {
        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public string Alias { get; set; }
    }
}
