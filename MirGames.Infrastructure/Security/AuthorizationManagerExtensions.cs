namespace MirGames.Infrastructure.Security
{
    using System.Security;
    using System.Security.Claims;

    /// <summary>
    /// Extensions of the authorization manager.
    /// </summary>
    public static class AuthorizationManagerExtensions
    {
        /// <summary>
        /// Ensures the access.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="action">The action.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.Security.SecurityException">Access denied.</exception>
        public static void EnsureAccess(this IAuthorizationManager authorizationManager, ClaimsPrincipal principal, string action, object resource)
        {
            if (!authorizationManager.CheckAccess(principal, action, resource))
            {
                throw new SecurityException("Access denied");
            }
        }
    }
}