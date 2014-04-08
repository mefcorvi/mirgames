namespace MirGames.Infrastructure
{
    using System;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Represents a data read context.
    /// </summary>
    public interface IReadContext : IDisposable
    {
        /// <summary>
        /// Creates a new query.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <returns>The query.</returns>
        DbQuery<T> Query<T>() where T : class;
    }
}
