namespace MirGames.Domain.Users.Entities
{
    /// <summary>
    /// Stores identifier of administrator user.
    /// </summary>
    internal class UserAdministrator
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }
}
