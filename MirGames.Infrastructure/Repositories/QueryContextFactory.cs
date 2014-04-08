namespace MirGames.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The read context factory.
    /// </summary>
    internal sealed class QueryContextFactory : IReadContextFactory
    {
        /// <summary>
        /// The mappers.
        /// </summary>
        private readonly IEnumerable<IEntityMapper> mappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryContextFactory" /> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public QueryContextFactory(IEnumerable<IEntityMapper> mappers)
        {
            Contract.Requires(mappers != null);
            this.mappers = mappers;
        }

        /// <inheritdoc />
        public IReadContext Create()
        {
            return new ReadContext(new DataContext(this.mappers));
        }
    }
}