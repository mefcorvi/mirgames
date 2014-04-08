namespace MirGames.Services.Git.Services
{
    using System.IO;

    using MirGames.Infrastructure;

    internal sealed class RepositoryPathProvider : IRepositoryPathProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryPathProvider"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RepositoryPathProvider(ISettings settings)
        {
            this.settings = settings;
        }

        /// <inheritdoc />
        public string GetPath(string repositoryName)
        {
            var repositoriesPath = this.settings.GetValue<string>("Services.Git.RepositoriesPath");
            return Path.Combine(repositoriesPath, repositoryName);
        }
    }
}