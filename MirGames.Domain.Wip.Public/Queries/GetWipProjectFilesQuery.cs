namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    public sealed class GetWipProjectFilesQuery : Query<WipProjectRepositoryItemViewModel>
    {
        public string Alias { get; set; }

        public string RelativePath { get; set; }
    }
}