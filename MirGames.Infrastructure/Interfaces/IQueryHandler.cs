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