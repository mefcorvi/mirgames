// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetIsProjectNameUniqueQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    internal sealed class GetIsProjectNameUniqueQueryHandler : SingleItemQueryHandler<GetIsProjectNameUniqueQuery, bool>
    {
        /// <inheritdoc />
        public override bool Execute(IReadContext readContext, GetIsProjectNameUniqueQuery query, ClaimsPrincipal principal)
        {
            return !readContext
                .Query<Project>()
                .Any(p => p.Alias == query.Alias);
        }
    }
}
