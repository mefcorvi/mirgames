// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipTagsQueryHandler.cs">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetWipTagsQuery.
    /// </summary>
    internal sealed class GetWipTagsQueryHandler : QueryHandler<GetWipTagsQuery, string>
    {
        /// <inheritdoc />
        protected override IEnumerable<string> Execute(IReadContext readContext, GetWipTagsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.GetTagsQuery(readContext).Take(20).OrderBy(t => t);
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetWipTagsQuery query, ClaimsPrincipal principal)
        {
            return this.GetTagsQuery(readContext).Count();
        }

        /// <summary>
        /// Gets the tags query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <returns>
        /// The tags query.
        /// </returns>
        private IQueryable<string> GetTagsQuery(IReadContext readContext)
        {
            return readContext
                .Query<ProjectTag>()
                .GroupBy(t => t.TagText, (tag, rows) => new { Tag = tag, Count = rows.Count() })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Tag);
        }
    }
}