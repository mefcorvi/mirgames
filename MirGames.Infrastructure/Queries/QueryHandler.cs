// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryHandler.cs">
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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    /// <typeparam name="T">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class QueryHandler<T, TResult> : IQueryHandler<TResult> where T : Query<TResult>
    {
        /// <inheritdoc />
        public Type QueryType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        IEnumerable IQueryHandler.Execute(IReadContext readContext, Query query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);
            
            return ((IQueryHandler<TResult>)this).Execute(readContext, (T)query, principal, pagination);
        }

        /// <inheritdoc />
        int IQueryHandler.GetItemsCount(IReadContext readContext, Query query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);

            return this.GetItemsCount(readContext, (T)query, principal);
        }

        /// <inheritdoc />
        IEnumerable<TResult> IQueryHandler<TResult>.Execute(IReadContext readContext, Query<TResult> query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);

            return this.Execute(readContext, (T)query, principal, pagination).EnsureCollection();
        }

        /// <summary>
        /// Applies the pagination.
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="queryable">The set of items.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The paginated query.</returns>
        protected IQueryable<TItem> ApplyPagination<TItem>(IQueryable<TItem> queryable, PaginationSettings pagination)
        {
            return pagination == null ? queryable : queryable.Skip(pagination.PageSize * pagination.PageNum).Take(pagination.PageSize);
        }

        /// <summary>
        /// Applies the pagination.
        /// </summary>
        /// <typeparam name="TItem">Type of item.</typeparam>
        /// <param name="queryable">The set of items.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The paginated query.</returns>
        protected IEnumerable<TItem> ApplyPagination<TItem>(IEnumerable<TItem> queryable, PaginationSettings pagination)
        {
            return pagination == null ? queryable : queryable.Skip(pagination.PageSize * pagination.PageNum).Take(pagination.PageSize);
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The items count.</returns>
        protected abstract int GetItemsCount(IReadContext readContext, T query, ClaimsPrincipal principal);

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The set of result items.</returns>
        protected abstract IEnumerable<TResult> Execute(IReadContext readContext, T query, ClaimsPrincipal principal, PaginationSettings pagination);
    }
}