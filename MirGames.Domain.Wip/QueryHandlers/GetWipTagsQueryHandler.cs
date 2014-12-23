// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipTagsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetWipTagsQuery.
    /// </summary>
    internal sealed class GetWipTagsQueryHandler : QueryHandler<GetWipTagsQuery, WipTagViewModel>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipTagsQueryHandler"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetWipTagsQueryHandler(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override IEnumerable<WipTagViewModel> Execute(GetWipTagsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.ApplyPagination(this.GetTagsQuery(readContext, query), pagination).ToList();
            }
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetWipTagsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetTagsQuery(readContext, query).Count();
            }
        }

        /// <summary>
        /// Gets the tags query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The tags query.
        /// </returns>
        private IQueryable<WipTagViewModel> GetTagsQuery(IReadContext readContext, GetWipTagsQuery query)
        {
            var set = readContext
                .Query<ProjectTag>();

            if (!string.IsNullOrEmpty(query.Filter))
            {
                set = set.Where(q => q.TagText.StartsWith(query.Filter));
            }

            return set
                .GroupBy(t => t.TagText, (tag, rows) => new { Tag = tag, Count = rows.Count() })
                .OrderByDescending(t => t.Count)
                .Select(t => new WipTagViewModel
                {
                    Tag = t.Tag,
                    Count = t.Count
                });
        }
    }
}