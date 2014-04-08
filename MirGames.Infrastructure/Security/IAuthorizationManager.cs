namespace MirGames.Infrastructure.Security
{
    using System.Security.Claims;

    /// <summary>
    /// Claims-based authorization.
    /// </summary>
    public interface IAuthorizationManager
    {
        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="action">The action.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>
        /// True when authorized, otherwise false.
        /// </returns>
        bool CheckAccess(ClaimsPrincipal principal, string action, object resource);
    }
}