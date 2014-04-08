namespace MirGames.OAuth
{
    using DotNetOpenAuth.AspNet;

    /// <summary>
    /// Provides the authentication client.
    /// </summary>
    public interface IAuthenticationClientProvider
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns>The properly configured authentication client.</returns>
        IAuthenticationClient GetClient();
    }
}