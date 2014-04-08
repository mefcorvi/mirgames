namespace MirGames.Infrastructure.Queries
{
    using System;

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public interface IQueryItemPostProcessor<in T> : IQueryItemPostProcessor
    {
        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Process(T item);
    }

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    public interface IQueryItemPostProcessor
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Process(object item);
    }

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public abstract class QueryItemPostProcessor<T> : IQueryItemPostProcessor<T>
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        public Type ItemType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        void IQueryItemPostProcessor<T>.Process(T item)
        {
            this.Process(item);
        }

        /// <inheritdoc />
        void IQueryItemPostProcessor.Process(object item)
        {
            this.Process((T)item);
        }

        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The result of processing.</returns>
        protected abstract void Process(T item);
    }
}
