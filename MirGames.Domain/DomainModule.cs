namespace MirGames.Domain
{
    using System.Security.Claims;

    using Autofac;

    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Settings;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class DomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Md5PasswordHashProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TextHashProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MarkdownShortTextExtractor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommonTextTransform>().AsImplementedInterfaces().SingleInstance();
            builder.Register(c => ClaimsPrincipal.Current).As<ClaimsPrincipal>().InstancePerDependency();

            builder.RegisterType<AppSettings>().Named<ISettings>("settings").SingleInstance();

            builder.RegisterDecorator<ISettings>(
                (c, inner) => new DataStoredSettings(inner, c.Resolve<IReadContextFactory>()), fromKey: "settings", toKey: "dataStoredSettings");

            builder.RegisterDecorator<ISettings>(
                (c, inner) => new CachedSettings(inner, c.Resolve<ICacheManager>()), fromKey: "dataStoredSettings");
        }
    }
}
