namespace MirGames.Domain.Users.Security
{
    using System;
    using System.Security.Claims;

    /// <summary>
    /// Stores the principal in cache.
    /// </summary>
    public interface IPrincipalCache
    {
        /// <summary>
        /// Gets or adds the principal.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="principalFactory">The principal factory.</param>
        /// <returns>The principal.</returns>
        ClaimsPrincipal GetOrAdd(string sessionId, Func<ClaimsPrincipal> principalFactory);

        /// <summary>
        /// Removes the specified session unique identifier.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        void Remove(string sessionId);
    }
}
