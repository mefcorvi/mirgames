namespace MirGames.Infrastructure
{
    using System.Security.Claims;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Abstraction of the query that returns only a one item.
    /// </summary>
    /// <typeparam name="TResult">Type of entity.</typeparam>
    public interface ISingleItemQueryHandler<TResult> : IQueryHandler<TResult>
    {
        /// <summary>
        /// Executes the current query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The query result.</returns>
        TResult Execute(IReadContext readContext, SingleItemQuery<TResult> query, ClaimsPrincipal principal);
    }
}