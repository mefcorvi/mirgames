namespace MirGames.Domain.Chat.ClaimsProviders
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The forum claims provider.
    /// </summary>
    internal sealed class ChatClaimsProvider : IAdditionalClaimsProvider
    {
        /// <inheritdoc />
        public IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal)
        {
            if (principal.IsInRole("User"))
            {
                yield return new Claim(ClaimTypes.Role, "ChatMember");
            }

            if (principal.IsInRole("Administrator"))
            {
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Edit", "ChatMessage");
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Delete", "ChatMessage");
            }
        }
    }
}
