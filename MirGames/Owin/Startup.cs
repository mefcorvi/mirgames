using Microsoft.Owin;
using MirGames.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MirGames.Owin
{
    using global::Owin;

    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            app.Use<NewRelicMiddleware>();
            app.MapSignalR();
        }
    }
}