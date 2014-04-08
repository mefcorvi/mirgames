namespace MirGames.Services.Git.Public.Queries
{
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    public sealed class GetRepositoryFileQuery : SingleItemQuery<GitRepositoryFileViewModel>
    {
        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }
    }
}