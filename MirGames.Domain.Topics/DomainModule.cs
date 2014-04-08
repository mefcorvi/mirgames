namespace MirGames.Domain.Topics
{
    using Autofac;

    using MirGames.Domain.Topics.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class TopicsDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UploadProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
