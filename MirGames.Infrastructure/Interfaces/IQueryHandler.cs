// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The query handler.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<TResult> : IQueryHandler
    {
        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The set of result items.</returns>
        IEnumerable<TResult> Execute(IReadContext readContext, Query<TResult> query, ClaimsPrincipal principal, PaginationSettings pagination);
    }

    /// <summary>
    /// The query handler.
    /// </summary>
    public interface IQueryHandler
    {
        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        Type QueryType { get; }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The set of result items.</returns>
        IEnumerable Execute(IReadContext readContext, Query query, ClaimsPrincipal principal, PaginationSettings pagination);

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The items count.</returns>
        int GetItemsCount(IReadContext readContext, Query query, ClaimsPrincipal principal);
    }
}