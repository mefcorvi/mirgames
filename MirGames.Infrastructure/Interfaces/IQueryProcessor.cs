namespace MirGames.Infrastructure
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Represents the query processor.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>A set of items.</returns>
        IEnumerable<T> Process<T>(Query<T> query, PaginationSettings pagination = null);

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Count of items.</returns>
        int GetItemsCount(Query query);

        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>An item.</returns>
        T Process<T>(SingleItemQuery<T> query);

        /// <summary>
        /// Processes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The query result.</returns>
        IEnumerable<object> Process(Query query, PaginationSettings pagination);
    }
}