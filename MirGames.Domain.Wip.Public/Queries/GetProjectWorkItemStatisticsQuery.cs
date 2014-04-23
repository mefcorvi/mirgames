namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns statistic of the project work items.
    /// </summary>
    public sealed class GetProjectWorkItemStatisticsQuery : SingleItemQuery<ProjectWorkItemStatisticsViewModel>
    {
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        public string ProjectAlias { get; set; }
    }
}