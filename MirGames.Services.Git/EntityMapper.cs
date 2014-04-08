namespace MirGames.Services.Git
{
    using MirGames.Infrastructure;
    using MirGames.Services.Git.Mapping;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new RepositoryMap());
            registry.Register(new RepositoryAccessMap());
        }
    }
}