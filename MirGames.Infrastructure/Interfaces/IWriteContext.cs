namespace MirGames.Infrastructure
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Represents the write context.
    /// </summary>
    public interface IWriteContext : IDisposable
    {
        /// <summary>
        /// Returns the set of items.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <returns>The set of items.</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
    }
}