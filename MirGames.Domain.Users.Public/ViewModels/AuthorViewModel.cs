namespace MirGames.Domain.Users.ViewModels
{
    /// <summary>
    /// The view model of author.
    /// </summary>
    public sealed class AuthorViewModel
    {
        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int? Id { get; set; }
    }
}