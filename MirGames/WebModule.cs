using MirGames.Services.Git;

namespace MirGames
{
    using System.Reflection;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.SignalR;
    using Autofac.Integration.WebApi;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain;
    using MirGames.Domain.Attachments;
    using MirGames.Domain.Chat;
    using MirGames.Domain.Forum;
    using MirGames.Domain.Tools;
    using MirGames.Domain.Topics;
    using MirGames.Domain.Users;
    using MirGames.Domain.Wip;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.OAuth;

    using RouteJs;

    using Module = Autofac.Module;

    /// <summary>
    /// The web module.
    /// </summary>
    public sealed class WebModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();

            builder.RegisterInstance(RouteTable.Routes).As<RouteCollection>();
            builder.RegisterInstance(new ClaimsPrincipalUserIdProvider()).As<IUserIdProvider>();

            builder.RegisterModule<RouteJsModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<UsersDomainModule>();
            builder.RegisterModule<AttachmentsDomainModule>();
            builder.RegisterModule<ForumDomainModule>();
            builder.RegisterModule<TopicsDomainModule>();
            builder.RegisterModule<ChatDomainModule>();
            builder.RegisterModule<ToolsDomainModule>();
            builder.RegisterModule<WipDomainModule>();
            builder.RegisterModule<GitModule>();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IAuthenticationClientProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IEventListener>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientHostAddressProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
