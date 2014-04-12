// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetOAuthProvidersQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the OAuth providers.
    /// </summary>
    internal sealed class GetOAuthProvidersQueryHandler : QueryHandler<GetOAuthProvidersQuery, OAuthProviderViewModel>
    {
        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetOAuthProvidersQuery query, ClaimsPrincipal principal)
        {
            return readContext.Query<OAuthProvider>().Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<OAuthProviderViewModel> Execute(IReadContext readContext, GetOAuthProvidersQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var providers = readContext
                .Query<OAuthProvider>()
                .OrderBy(p => p.Id)
                .Select(
                    p => new OAuthProviderViewModel
                        {
                            DisplayName = p.DisplayName,
                            ProviderName = p.Name,
                            IsLinked = false,
                            ProviderId = p.Id
                        })
                .ToList();

            if (principal.GetUserId().HasValue)
            {
                int userId = principal.GetUserId().GetValueOrDefault();

                var linkedProviders = readContext
                    .Query<OAuthToken>()
                    .Where(t => t.UserId == userId)
                    .Select(p => p.ProviderId)
                    .ToList();

                providers.ForEach(p => p.IsLinked = linkedProviders.Contains(p.ProviderId));
            }

            return providers;
        }
    }
}
