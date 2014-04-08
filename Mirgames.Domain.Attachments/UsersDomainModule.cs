namespace MirGames.Domain.Attachments
{
    using Autofac;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class AttachmentsDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
