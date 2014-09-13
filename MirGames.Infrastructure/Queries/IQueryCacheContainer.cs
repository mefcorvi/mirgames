// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IQueryCacheContainer.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.Queries
{
    using System;
    using System.Collections;
    using System.Security.Claims;

    /// <summary>
    /// Manages the query cache.
    /// </summary>
    internal interface IQueryCacheContainer
    {
        /// <summary>
        /// Determines whether this instance can handle the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>True whether this instance can handle the specified query.</returns>
        bool CanHandle(Query query);

        /// <summary>
        /// Gets or adds the query result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="resultFactory">The result factory.</param>
        /// <returns>The query result.</returns>
        IEnumerable GetOrAdd(
            Query query,
            ClaimsPrincipal principal,
            PaginationSettings pagination,
            Func<IEnumerable> resultFactory);

        /// <summary>
        /// Gets the or add items count.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="resultFactory">The result factory.</param>
        /// <returns>Count of items.</returns>
        int GetOrAddItemsCount(
            Query query,
            ClaimsPrincipal principal,
            Func<int> resultFactory);
    }
}