namespace System.Web.Mvc
{
    using System.IO;
    using System.Web.Routing;

    /// <summary>
    /// The route info.
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteInfo"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public RouteInfo(RouteData data)
        {
            this.RouteData = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public RouteInfo(Uri uri)
        {
            var request = new HttpRequest(null, uri.AbsoluteUri, uri.Query.Trim('?'));
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            this.RouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        public RouteData RouteData { get; private set; }
    }
}