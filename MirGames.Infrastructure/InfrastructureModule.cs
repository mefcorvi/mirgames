namespace MirGames.Infrastructure
{
    using Autofac;

    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Notifications;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Repositories;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Transactions;
    using MirGames.Infrastructure.Utilities;

    /// <summary>
    /// The infrastructure module.
    /// </summary>
    public class InfrastructureModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<QueryContextFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WriteContextFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QueryProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommandProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<NotificationManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RazorTemplateProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SearchEngine.SearchEngine>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<EventLog>().As<IEventLog>().SingleInstance();
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterType<ContentTypeProvider>().As<IContentTypeProvider>().SingleInstance();
            builder.RegisterType<TransactionExecutor>().As<ITransactionExecutor>().SingleInstance();

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>().SingleInstance();
        }
    }
}
