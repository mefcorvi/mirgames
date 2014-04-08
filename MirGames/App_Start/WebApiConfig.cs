using System.Web.Http;

namespace MirGames
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(name: "QueryAllApi", routeTemplate: "api/all", defaults: new { controller = "WebApi", action = "GetAll" });
            config.Routes.MapHttpRoute(name: "QueryOneApi", routeTemplate: "api/one", defaults: new { controller = "WebApi", action = "GetOne" });
            config.Routes.MapHttpRoute(name: "CommandApi", routeTemplate: "api", defaults: new { controller = "WebApi", action = "Post" });
        }
    }
}
