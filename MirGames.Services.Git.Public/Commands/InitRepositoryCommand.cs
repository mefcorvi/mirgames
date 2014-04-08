namespace MirGames.Services.Git.Public.Commands
{
    using MirGames.Infrastructure.Commands;

    public sealed class InitRepositoryCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        public string RepositoryName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}