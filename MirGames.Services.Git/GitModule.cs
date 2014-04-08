namespace MirGames.Services.Git
{
    using Autofac;

    using MirGames.Domain;
    using MirGames.Services.Git.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class GitModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<RepositoryPathProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RepositorySecurity>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GitService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
