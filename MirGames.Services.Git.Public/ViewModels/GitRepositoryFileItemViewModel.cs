namespace MirGames.Services.Git.Public.ViewModels
{
    public class GitRepositoryFileItemViewModel
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
        public GitRepositoryFileItemType ItemType { get; set; }
    }
}