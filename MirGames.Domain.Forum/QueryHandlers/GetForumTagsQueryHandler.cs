// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTagsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetMainTagsQuery.
    /// </summary>
    internal sealed class GetForumTagsQueryHandler : QueryHandler<GetForumTagsQuery, string>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumTagsQueryHandler"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetForumTagsQueryHandler(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override IEnumerable<string> Execute(GetForumTagsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetTagsQuery(readContext).Take(20).OrderBy(t => t);
            }
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetForumTagsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetTagsQuery(readContext).Count();
            }
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
                .Query<ForumTag>()
                .GroupBy(t => t.TagText, (tag, rows) => new { Tag = tag, Count = rows.Count() })
                .OrderByDescending(t => t.Count)
                .Select(t => t.Tag);
        }
    }
}