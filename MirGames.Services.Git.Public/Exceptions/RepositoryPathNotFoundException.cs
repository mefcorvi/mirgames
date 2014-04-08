namespace MirGames.Services.Git.Public.Exceptions
{
    using System;

    public class RepositoryPathNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryPathNotFoundException"/> class.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        public RepositoryPathNotFoundException(string relativePath)
        {
            this.RelativePath = relativePath;
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        public string RelativePath { get; private set; }
    }
}