namespace MirGames.Domain.Wip.Commands
{
    using System.IO;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Archives the project repository.
    /// </summary>
    public sealed class ArchiveProjectRepositoryCommand : Command
    {
        /// <summary>
        /// Gets or sets the project alias.
        /// </summary>
        public string ProjectAlias { get; set; }

        /// <summary>
        /// Gets or sets the output stream.
        /// </summary>
        public Stream OutputStream { get; set; }
    }
}