// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SingleItemQueryHandler.cs">
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
    using System.Security.Claims;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    /// <typeparam name="T">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class SingleItemQueryHandler<T, TResult> : IQueryHandler<TResult> where T : SingleItemQuery<TResult>
    {
        /// <inheritdoc />
        public Type QueryType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        IEnumerable<TResult> IQueryHandler<TResult>.Execute(IReadContext readContext, Query<TResult> query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);
            return new[] { this.Execute(readContext, (T)query, principal) };
        }

        /// <inheritdoc />
        IEnumerable IQueryHandler.Execute(IReadContext readContext, Query query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);
            return new[] { this.Execute(readContext, (T)query, principal) };
        }

        /// <inheritdoc />
        int IQueryHandler.GetItemsCount(IReadContext readContext, Query query, ClaimsPrincipal principal)
        {
            return 1;
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The result.</returns>
        protected abstract TResult Execute(IReadContext readContext, T query, ClaimsPrincipal principal);
    }
}