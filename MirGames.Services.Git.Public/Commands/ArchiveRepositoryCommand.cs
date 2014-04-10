namespace MirGames.Services.Git.Public.Commands
{
    using System.IO;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Archives the repository.
    /// </summary>
    public sealed class ArchiveRepositoryCommand : Command
    {
        /// <summary>
        /// Gets or sets the repository identifier.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets the output stream.
        /// </summary>
        public Stream OutputStream { get; set; }
    }
}