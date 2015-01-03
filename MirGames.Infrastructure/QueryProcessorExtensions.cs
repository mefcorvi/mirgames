// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryProcessorExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Extensions of the <see cref="IQueryProcessor"/> implementation.
    /// </summary>
    public static class QueryProcessorExtensions
    {
        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The result of query.</returns>
        public static IEnumerable<T> Process<T>(this IQueryProcessor queryProcessor, Query<T> query, PaginationSettings pagination = null)
        {
            return queryProcessor.Process<T>(query, pagination);
        }

        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The query result.</returns>
        public static IEnumerable<object> Process(this IQueryProcessor queryProcessor, Query query, PaginationSettings pagination)
        {
            return queryProcessor.Process<object>(query, pagination);
        }

        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        public static T Process<T>(this IQueryProcessor queryProcessor, SingleItemQuery<T> query)
        {
            return queryProcessor.Process<T>(query).FirstOrDefault();
        }
    }
}
