namespace MirGames.Infrastructure.Security
{
    using System.Collections.Generic;
    using System.Security.Claims;

    /// <summary>
    /// The additional claims provider.
    /// </summary>
    public interface IAdditionalClaimsProvider
    {
        /// <summary>
        /// Gets the additional claims.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The set of additional claims.</returns>
        IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal);
    }
}