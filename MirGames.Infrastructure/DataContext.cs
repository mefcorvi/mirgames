namespace MirGames.Infrastructure
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure.Repositories;

    /// <summary>
    /// Represents the data context.
    /// </summary>
    internal class DataContext : DbContext
    {
        /// <summary>
        /// My trace source.
        /// </summary>
        private static readonly TraceSource TraceSource = new TraceSource("DataContext", SourceLevels.All);

        /// <summary>
        /// The entity mappers.
        /// </summary>
        private readonly IEnumerable<IEntityMapper> mappers;

        /// <summary>
        /// Initializes static members of the <see cref="DataContext"/> class.
        /// </summary>
        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public DataContext(IEnumerable<IEntityMapper> mappers)
            : base("Name=MirGames")
        {
            Contract.Requires(mappers != null);
            
            this.mappers = mappers;
            this.Database.Log = s => TraceSource.TraceInformation(s);
            
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized +=
                (sender, e) => DateTimeKindAttribute.Apply(e.Entity);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var registrty = new EntityMappingRegistry(modelBuilder);
            this.mappers.ForEach(m => m.Configure(registrty));

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
