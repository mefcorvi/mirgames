namespace MirGames.Domain.Topics.ClaimsProviders
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The forum claims provider.
    /// </summary>
    internal sealed class TopicsClaimsProvider : IAdditionalClaimsProvider
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicsClaimsProvider"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public TopicsClaimsProvider(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal)
        {
            if (this.authorizationManager.CheckAccess(principal, "Create", new Topic()))
            {
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Create", "Topic");
            }
        }
    }
}
