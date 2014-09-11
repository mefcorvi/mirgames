// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetMainTagsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetMainTagsQuery.
    /// </summary>
    internal sealed class GetMainTagsQueryHandler : QueryHandler<GetMainTagsQuery, string>
    {
        /// <inheritdoc />
        protected override IEnumerable<string> Execute(IReadContext readContext, GetMainTagsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.GetTagsQuery(readContext, query).Take(20).OrderBy(t => t);
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetMainTagsQuery query, ClaimsPrincipal principal)
        {
            return this.GetTagsQuery(readContext, query).Count();
        }

        /// <summary>
        /// Gets the tags query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The tags query.
        /// </returns>
        private IQueryable<string> GetTagsQuery(IReadContext readContext, GetMainTagsQuery query)
        {
            var tags = readContext
                .Query<TopicTag>();

            if (!string.IsNullOrEmpty(query.Filter))
            {
                tags = tags.Where(t => t.TagText.StartsWith(query.Filter));
            }

            return tags
                .GroupBy(t => t.TagText, (tag, rows) => new { Tag = tag, Count = rows.Count() })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Tag);
        }
    }
}