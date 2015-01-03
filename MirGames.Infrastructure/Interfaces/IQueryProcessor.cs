// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IQueryProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Represents the query processor.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Count of items.</returns>
        int GetItemsCount(Query query);

        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <typeparam name="T">Type of the result item.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The query result.</returns>
        IEnumerable<T> Process<T>(Query query, PaginationSettings pagination = null);

        /// <summary>
        /// Processes the query asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The query result.</returns>
        Task<ICollection<T>> ProcessAsync<T>(Query query, PaginationSettings pagination = null);
    }
}