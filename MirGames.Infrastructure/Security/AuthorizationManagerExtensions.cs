// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AuthorizationManagerExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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