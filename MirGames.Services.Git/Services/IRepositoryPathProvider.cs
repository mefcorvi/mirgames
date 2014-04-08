namespace MirGames.Services.Git.Services
{
    /// <summary>
    /// Provides path to the repository.
    /// </summary>
    internal interface IRepositoryPathProvider
    {
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>The repository path.</returns>
        string GetPath(string repositoryName);
    }
}