namespace MirGames.Domain.Wip.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Creates the new project.
    /// </summary>
    [Api]
    public sealed class CreateNewWipProjectCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the type of the repository.
        /// </summary>
        public string RepositoryType { get; set; }

        /// <summary>
        /// Gets or sets the logo attachment identifier.
        /// </summary>
        public int LogoAttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public int[] Attachments { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}
