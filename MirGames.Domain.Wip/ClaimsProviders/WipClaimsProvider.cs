// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WipClaimsProvider.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
