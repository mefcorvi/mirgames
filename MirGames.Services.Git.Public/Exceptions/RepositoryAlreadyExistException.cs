namespace MirGames.Services.Git.Public.Exceptions
{
    using System;

    public sealed class RepositoryAlreadyExistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryAlreadyExistException"/> class.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        public RepositoryAlreadyExistException(string repositoryName)
            : base(string.Format("Repository with the same name \"{0}\" already exist", repositoryName))
        {
            this.RepositoryName = repositoryName;
        }

        /// <summary>
        /// Gets the name of the repository.
        /// </summary>
        public string RepositoryName { get; private set; }
    }
}