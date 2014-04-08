namespace MirGames.Infrastructure.Exception
{
    using System;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Exception raised when query processing is failed.
    /// </summary>
    public class QueryProcessingFailedException : MirGamesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessingFailedException"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QueryProcessingFailedException(Query query) : this(query, null)
        {
            this.Query = query;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessingFailedException"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="innerException">The inner exception.</param>
        public QueryProcessingFailedException(Query query, Exception innerException)
            : base("Processing of query was failed. Type of query: " + query.GetType().Name, innerException)
        {
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        public Query Query { get; private set; }
    }
}