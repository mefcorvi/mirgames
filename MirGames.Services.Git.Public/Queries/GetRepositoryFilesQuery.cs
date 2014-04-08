namespace MirGames.Services.Git.Public.Queries
{
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    public sealed class GetRepositoryFilesQuery : Query<GitRepositoryFileItemViewModel>
    {
        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        public string RelativePath { get; set; }
    }
}