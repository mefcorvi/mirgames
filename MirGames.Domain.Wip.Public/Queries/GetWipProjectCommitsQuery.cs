namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns commits of the project.
    /// </summary>
    public sealed class GetWipProjectCommitsQuery : Query<WipProjectCommitViewModel>
    {
        public string Alias { get; set; }
    }
}