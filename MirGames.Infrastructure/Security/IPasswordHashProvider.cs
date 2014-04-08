namespace MirGames.Infrastructure.Security
{
    /// <summary>
    /// Provides the hash string of password.
    /// </summary>
    public interface IPasswordHashProvider
    {
        /// <summary>
        /// Gets the hash of the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>
        /// The hash of the password.
        /// </returns>
        string GetHash(string password, string salt);
    }
}
