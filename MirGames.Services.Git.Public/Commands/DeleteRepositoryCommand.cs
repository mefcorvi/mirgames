namespace MirGames.Services.Git.Public.Commands
{
    using MirGames.Infrastructure.Commands;

    public sealed class DeleteRepositoryCommand : Command
    {
        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        public string RepositoryName { get; set; }
    }
}