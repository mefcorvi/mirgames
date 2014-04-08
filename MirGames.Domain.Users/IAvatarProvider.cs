namespace MirGames.Domain.Users
{
    /// <summary>
    /// Provides an URL to avatar.
    /// </summary>
    public interface IAvatarProvider
    {
        /// <summary>
        /// Gets the avatar URL.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <param name="login">The login.</param>
        /// <returns>The URL.</returns>
        string GetAvatarUrl(string mail, string login);
    }
}
