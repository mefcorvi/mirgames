namespace RouteJs
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// ASP.NET handler for RouteJS. Renders JavaScript to handle routing of ASP.NET URLs
    /// </summary>
    public class RouteJsHandler : IHttpHandler
    {
        /// <summary>
        /// Gets the URL to the RouteJS handler, with a unique hash in the URL. The hash will change 
        /// every time a route changes
        /// </summary>
        public static string HandlerUrl
        {
            get { return DependencyResolver.Current.GetService<InternalRouteJsHandler>().GetHandlerUrl(); }
        }

        /// <inheritdoc />
        public bool IsReusable
        {
            get { return true; }
        }

        /// <inheritdoc />
        public void ProcessRequest(HttpContext context)
        {
            var handler = DependencyResolver.Current.GetService<InternalRouteJsHandler>();
            handler.ProcessRequest(context);
        }
    }
}
