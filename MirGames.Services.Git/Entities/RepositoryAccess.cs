namespace MirGames.Services.Git.Entities
{
    internal sealed class RepositoryAccess
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the repository identifier.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        public RepositoryAccessLevel AccessLevel { get; set; }
    }
}