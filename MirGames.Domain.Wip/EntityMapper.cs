namespace MirGames.Domain.Wip
{
    using MirGames.Domain.Wip.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new ProjectMap());
            registry.Register(new ProjectTagMap());
            registry.Register(new ProjectWorkItemMap());
        }
    }
}