namespace MirGames.Infrastructure.Security
{
    using System.Security.Claims;

    /// <summary>
    /// Provides user credentials.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets the principal.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The principal.</returns>
        ClaimsPrincipal GetPrincipal(string sessionId);
    }
}