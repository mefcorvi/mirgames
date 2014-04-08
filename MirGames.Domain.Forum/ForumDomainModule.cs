namespace MirGames.Domain.Forum
{
    using Autofac;

    using MirGames.Domain.Forum.Services;
    using MirGames.Domain.Forum.UserSettings;
    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class ForumDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<UseContiniousPaginationSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<ForumPostUploadProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
