namespace MirGames.Domain.Wip
{
    using Autofac;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Domain.Wip.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class WipDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ProjectLogoUploadProcessor>().As<IUploadProcessor>().SingleInstance();
        }
    }
}
