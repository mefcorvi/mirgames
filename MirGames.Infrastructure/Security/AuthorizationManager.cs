namespace MirGames.Infrastructure.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// Provides direct access methods for evaluating authorization policy
    /// </summary>
    internal class AuthorizationManager : IAuthorizationManager
    {
        /// <summary>
        /// The rules.
        /// </summary>
        private readonly ILookup<Type, IAccessRule> rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationManager"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public AuthorizationManager(IEnumerable<IAccessRule> rules)
        {
            this.rules = rules.ToLookup(rule => rule.ResourceType);
        }

        /// <inheritdoc />
        public bool CheckAccess(ClaimsPrincipal principal, string action, object resource)
        {
            if (principal.IsInRole("Administrator"))
            {
                return true;
            }

            var resourceType = resource.GetType();

            return this.rules[resourceType]
                .Where(rule => string.Equals(rule.Action, action, StringComparison.InvariantCultureIgnoreCase))
                .All(rule => rule.CheckAccess(principal, resource));
        }
    }
}