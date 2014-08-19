namespace MirGames.Domain.Wip.Commands
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Commands;

    [Api]
    public sealed class CreateNewProjectWorkItemCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        public string ProjectAlias { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public WorkItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public int[] Attachments { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        public int? AssignedTo { get; set; }
    }
}