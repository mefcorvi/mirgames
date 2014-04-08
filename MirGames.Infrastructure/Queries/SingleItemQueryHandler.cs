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
    public abstract class SingleItemQueryHandler<T, TResult> : ISingleItemQueryHandler<TResult> where T : SingleItemQuery<TResult>
    {
        /// <inheritdoc />
        public Type QueryType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The result.</returns>
        public abstract TResult Execute(IReadContext readContext, T query, ClaimsPrincipal principal);

        /// <inheritdoc />
        TResult ISingleItemQueryHandler<TResult>.Execute(IReadContext readContext, SingleItemQuery<TResult> query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);
            Contract.Requires(principal != null);
            return this.Execute(readContext, (T)query, principal);
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
    }
}