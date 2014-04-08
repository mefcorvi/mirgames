namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    public sealed class GetWipProjectQuery : SingleItemQuery<WipProjectViewModel>
    {
        /// <summary>
        /// Gets or sets the project unique identifier.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }
    }
}