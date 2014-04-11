namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the project identifier.
    /// </summary>
    public class GetProjectIdQuery : SingleItemQuery<int>
    {
        public string ProjectAlias { get; set; }
    }
}