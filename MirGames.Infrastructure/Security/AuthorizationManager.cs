// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AuthorizationManager.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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