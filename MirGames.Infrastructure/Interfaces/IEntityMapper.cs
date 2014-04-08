namespace MirGames.Infrastructure
{
    /// <summary>
    /// Represents the entity mapper.
    /// </summary>
    public interface IEntityMapper
    {
        /// <summary>
        /// Configures the mapping.
        /// </summary>
        /// <param name="registry">The mapping registry.</param>
        void Configure(IEntityMappingRegistry registry);
    }
}
