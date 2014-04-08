namespace MirGames.Services.Git.Public.Queries
{
    using global::MirGames.Infrastructure.Queries;

    using MirGames.Services.Git.Public.ViewModels;

    public sealed class GetRepositoryHistoryQuery : Query<GitRepositoryHistoryItemViewModel>
    {
        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        public int RepositoryId { get; set; }
    }
}
