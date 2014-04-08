namespace MirGames.Domain
{
    using Autofac;

    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Base class of the domain module.
    /// </summary>
    public abstract class DomainModuleBase : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var currentAssembly = this.GetType().Assembly;

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<ICommandHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IQueryHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IQueryItemPostProcessor>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IAccessRule>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IEventListener>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IAdditionalClaimsProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}