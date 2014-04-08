namespace MirGames.Domain.Topics
{
    using MirGames.Domain.Topics.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new BlogMap());
            registry.Register(new CommentMap());
            registry.Register(new FavoriteMap());
            registry.Register(new TopicContentMap());
            registry.Register(new TopicMap());
            registry.Register(new TopicTagMap());
        }
    }
}