namespace MirGames.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The write context factory.
    /// </summary>
    internal sealed class WriteContextFactory : IWriteContextFactory
    {
        /// <summary>
        /// The mappers.
        /// </summary>
        private readonly IEnumerable<IEntityMapper> mappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteContextFactory" /> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public WriteContextFactory(IEnumerable<IEntityMapper> mappers)
        {
            Contract.Requires(mappers != null);
            this.mappers = mappers;
        }

        /// <inheritdoc />
        public IWriteContext Create()
        {
            return new WriteContext(new DataContext(this.mappers));
        }
    }
}