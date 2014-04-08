namespace MirGames.Services.Git.Public.Queries
{
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    public sealed class GetRepositoryQuery : SingleItemQuery<GitRepositoryViewModel>
    {
        /// <summary>
        /// Gets or sets the repository identifier.
        /// </summary>
        public int RepositoryId { get; set; }
    }
}