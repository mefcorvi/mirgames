namespace MirGames.Infrastructure
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Represents the entity mapper registry.
    /// </summary>
    public interface IEntityMappingRegistry
    {
        /// <summary>
        /// Registers the mapping.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <param name="configuration">Mapping configuration.</param>
        void Register<T>(EntityTypeConfiguration<T> configuration) where T : class;
    }
}