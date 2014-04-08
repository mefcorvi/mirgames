namespace MirGames.Domain.Wip.ClaimsProviders
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The forum claims provider.
    /// </summary>
    internal sealed class WipClaimsProvider : IAdditionalClaimsProvider
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WipClaimsProvider"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public WipClaimsProvider(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal)
        {
            if (this.authorizationManager.CheckAccess(principal, "Create", new Project()))
            {
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Create", "WipProject");
            }
        }
    }
}
