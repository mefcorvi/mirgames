// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WipClaimsProvider.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.ClaimsProviders
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
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
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WipClaimsProvider" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public WipClaimsProvider(IAuthorizationManager authorizationManager, IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(authorizationManager != null);

            this.authorizationManager = authorizationManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        public IEnumerable<Claim> GetAdditionalClaims(ClaimsPrincipal principal)
        {
            var userId = principal.GetUserId();

            if (userId.HasValue)
            {
                using (var readContext = this.readContextFactory.Create())
                {
                    var projects = readContext
                        .Query<Project>()
                        .Where(p => p.AuthorId == userId.Value)
                        .Select(p => p.ProjectId)
                        .ToList();

                    foreach (var projectId in projects)
                    {
                        yield return ClaimsPrincipalExtensions.CreateActionClaim("CreateBug", "WipProject", projectId);
                        yield return ClaimsPrincipalExtensions.CreateActionClaim("CreateTask", "WipProject", projectId);
                        yield return ClaimsPrincipalExtensions.CreateActionClaim("CreateFeature", "WipProject", projectId);
                        yield return ClaimsPrincipalExtensions.CreateActionClaim("Edit", "WipProject", projectId);
                        yield return ClaimsPrincipalExtensions.CreateActionClaim("Delete", "WipProject", projectId);
                    }
                }
            }

            if (this.authorizationManager.CheckAccess(principal, "Create", new Project()))
            {
                yield return ClaimsPrincipalExtensions.CreateActionClaim("Create", "WipProject");
            }
        }
    }
}
