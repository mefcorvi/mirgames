namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Queries;

    public sealed class GetWipProjectFileQuery : SingleItemQuery<WipProjectFileViewModel>
    {
        public string Alias { get; set; }

        public string FilePath { get; set; }
    }
}