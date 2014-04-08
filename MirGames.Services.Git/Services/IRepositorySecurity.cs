namespace MirGames.Services.Git.Services
{
    public interface IRepositorySecurity
    {
        /// <summary>
        /// Determines whether this instance can write the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whther the specified repository could be writen.</returns>
        bool CanWrite(string repositoryName);

        /// <summary>
        /// Determines whether this instance can read the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whether the specified repository could be read.</returns>
        bool CanRead(string repositoryName);

        /// <summary>
        /// Determines whether this instance can delete the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whether the specified repository could be deleted.</returns>
        bool CanDelete(string repositoryName);
    }
}