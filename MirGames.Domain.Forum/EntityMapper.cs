namespace MirGames.Domain.Forum
{
    using MirGames.Domain.Forum.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new ForumTagMap());
            registry.Register(new ForumTopicMap());
            registry.Register(new ForumTopicReadMap());
            registry.Register(new ForumTopicUnreadMap());
            registry.Register(new ForumPostMap());
        }
    }
}