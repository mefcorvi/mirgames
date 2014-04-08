namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Context of reading.
    /// </summary>
    internal sealed class ReadContext : IReadContext
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadContext" /> class.
        /// </summary>
        /// <param name="context">The DB context.</param>
        public ReadContext(DbContext context)
        {
            Contract.Requires(context != null);
            this.context = context;
        }

        /// <inheritdoc />
        public DbQuery<T> Query<T>() where T : class
        {
            return this.context.Set<T>();
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
    }
}
