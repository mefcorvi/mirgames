// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetCurrentUserClaimsQueryHandler.cs">
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

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns claims of the current user.
    /// </summary>
    internal sealed class GetCurrentUserClaimsQueryHandler : QueryHandler<GetCurrentUserClaimsQuery, UserClaimViewModel>
    {
        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetCurrentUserClaimsQuery query, ClaimsPrincipal principal)
        {
            return principal.Claims.Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<UserClaimViewModel> Execute(IReadContext readContext, GetCurrentUserClaimsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.ApplyPagination(principal.Claims.OrderBy(c => c.Type), pagination).Select(
                c => new UserClaimViewModel
                    {
                        Type = c.Type,
                        Value = c.Value
                    });
        }
    }
}