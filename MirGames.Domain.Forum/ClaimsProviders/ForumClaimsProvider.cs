namespace MirGames.Domain.Forum.ClaimsProviders
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The forum claims provider.
    /// </summary>
    internal sealed class ForumClaimsProvider : IAdditionalClaimsProvider
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumClaimsProvider"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public ForumClaimsProvider(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal)
        {
            if (this.authorizationManager.CheckAccess(principal, "Create", new ForumTopic()))
            {
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Create", "ForumTopic");
            }
        }
    }
}
