namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    public sealed class GetProjectWorkItemsQuery : Query<ProjectWorkItemViewModel>
    {
        /// <summary>
        /// Gets or sets the project alias.
        /// </summary>
        public string ProjectAlias { get; set; }
    }
}
