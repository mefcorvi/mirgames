namespace MirGames.Domain.Attachments
{
    using MirGames.Domain.Attachments.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new AttachmentMap());
        }
    }
}