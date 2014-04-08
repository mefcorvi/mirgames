namespace MirGames.Infrastructure.Security
{
    using System;
    using System.Globalization;
    using System.Security.Claims;

    /// <summary>
    /// The claims provider.
    /// </summary>
    public static class ClaimsProvider
    {
        /// <summary>
        /// Gets the type of the action claim.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity unique identifier.</param>
        /// <returns>
        /// The action claim type.
        /// </returns>
        public static string GetActionClaimType(string entityType, int? entityId)
        {
            return string.Format(
                "http://mirgames.ru/claims/action/{0}/{1}",
                entityType,
                entityId);
        }

        /// <summary>
        /// Gets the action claim.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity unique identifier.</param>
        /// <param name="allowedActions">The allowed actions.</param>
        /// <param name="validThrough">The valid through date.</param>
        /// <returns>
        /// The action claim.
        /// </returns>
        public static Claim GetActionClaim(string entityType, int? entityId, string[] allowedActions, DateTime? validThrough)
        {
            string claimType = GetActionClaimType(entityType, entityId);
            var claim = new Claim(claimType, "allow");

            if (validThrough.HasValue)
            {
                claim.Properties["ValidThrough"] = validThrough.Value.ToString(CultureInfo.InvariantCulture);

                foreach (string allowedAction in allowedActions)
                {
                    claim.Properties[allowedAction + "Action"] = "Allow";
                }
            }

            return claim;
        }
    }
}
