namespace MirGames.Domain.Users
{
    using Autofac;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Domain.Users.Security;
    using MirGames.Domain.Users.Services;
    using MirGames.Domain.Users.UserSettings;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class UsersDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UserAvatarUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<TimeZoneSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AuthenticationProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AvatarProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PrincipalCache>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OnlineUsersManager>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
