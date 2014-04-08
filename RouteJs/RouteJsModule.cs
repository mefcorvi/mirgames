namespace RouteJs
{
    using System.Configuration;

    using Autofac;

    /// <summary>
    /// Register all Route JS components that are coupled with ASP.NET MVC
    /// </summary>
    public sealed class RouteJsModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IConfiguration>((c, o) => (RouteJsConfigurationSection)ConfigurationManager.GetSection("routeJs"));
            
            builder.RegisterType<IgnoreUnsupportedRoutesFilter>().As<IRouteFilter>();
            builder.RegisterType<MvcRouteFilter>().As<IRouteFilter>();
            
            builder.RegisterType<DefaultsProcessor>().As<IDefaultsProcessor>();
            builder.RegisterType<RouteJs>().As<RouteJs>();
            builder.RegisterType<InternalRouteJsHandler>().As<InternalRouteJsHandler>();
        }
    }
}
