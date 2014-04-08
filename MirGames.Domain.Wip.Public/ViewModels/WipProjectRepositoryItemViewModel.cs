namespace MirGames.Domain.Wip.ViewModels
{
    public class WipProjectRepositoryItemViewModel
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public WipProjectRepositoryItemType ItemType { get; set; }
    }
}