namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The entity mapping registry.
    /// </summary>
    internal sealed class EntityMappingRegistry : IEntityMappingRegistry
    {
        /// <summary>
        /// The model builder.
        /// </summary>
        private readonly DbModelBuilder modelBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMappingRegistry"/> class.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public EntityMappingRegistry(DbModelBuilder modelBuilder)
        {
            Contract.Requires(modelBuilder != null);
            this.modelBuilder = modelBuilder;
        }

        /// <summary>
        /// Registers mapping in the registry.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <typeparam name="T">Type of entity.</typeparam>
        public void Register<T>(EntityTypeConfiguration<T> configuration) where T : class
        {
            Contract.Requires(configuration != null);
            this.modelBuilder.Configurations.Add(configuration);
        }
    }
}