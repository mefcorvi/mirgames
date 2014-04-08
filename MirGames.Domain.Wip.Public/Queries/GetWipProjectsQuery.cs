namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the WIP projects.
    /// </summary>
    public sealed class GetWipProjectsQuery : Query<WipProjectViewModel>
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }
    }
}
