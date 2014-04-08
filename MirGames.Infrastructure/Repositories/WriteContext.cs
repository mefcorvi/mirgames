namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Context of writing.
    /// </summary>
    internal sealed class WriteContext : IWriteContext
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteContext"/> class.
        /// </summary>
        /// <param name="context">The DB context.</param>
        public WriteContext(DbContext context)
        {
            Contract.Requires(context != null);
            this.context = context;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
                this.context = null;
            }
        }

        /// <inheritdoc />
        public DbSet<T> Set<T>() where T : class
        {
            return this.context.Set<T>();
        }

        /// <inheritdoc />
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}