namespace RouteJs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    /// <summary>
    /// Main class for RouteJS. Handles retrieving the list of routes.
    /// </summary>
    internal sealed class RouteJs
    {
        /// <summary>
        /// The route collection.
        /// </summary>
        private readonly RouteCollection routeCollection;

        /// <summary>
        /// The route filters.
        /// </summary>
        private readonly IEnumerable<IRouteFilter> routeFilters;

        /// <summary>
        /// The defaults processors.
        /// </summary>
        private readonly IEnumerable<IDefaultsProcessor> defaultsProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteJs" /> class.
        /// </summary>
        /// <param name="routeCollection">The route collection.</param>
        /// <param name="routeFilters">Any filters to apply to the routes</param>
        /// <param name="defaultsProcessors">Handler to handle processing of default values</param>
        public RouteJs(RouteCollection routeCollection, IEnumerable<IRouteFilter> routeFilters, IEnumerable<IDefaultsProcessor> defaultsProcessors)
        {
            this.routeCollection = routeCollection;
            this.routeFilters = routeFilters;
            this.defaultsProcessors = defaultsProcessors;
        }

        /// <summary>
        /// Gets all the JavaScript-visible routes.
        /// </summary>
        /// <returns>A list of JavaScript-visible routes</returns>
        public IEnumerable<RouteInfo> GetRoutes()
        {
            var routes = this.routeCollection.GetRoutes();

            return routes.Where(this.AllowRoute).OfType<Route>().Select(this.GetRoute);
        }

        /// <summary>
        /// Check whether this route should be exposed in the JavaScript
        /// </summary>
        /// <param name="route">Route to check</param>
        /// <returns><c>true</c> if the route should be exposed</returns>
        private bool AllowRoute(RouteBase route)
        {
            return this.routeFilters.All(filter => filter.AllowRoute(route));
        }

        /// <summary>
        /// Gets information about the specified route
        /// </summary>
        /// <param name="route">The route</param>
        /// <returns>Route information</returns>
        private RouteInfo GetRoute(Route route)
        {
            var routeInfo = new RouteInfo
            {
                Url = route.Url,
                Constraints = route.Constraints ?? new RouteValueDictionary()
            };

            foreach (var processor in this.defaultsProcessors)
            {
                processor.ProcessDefaults(route, routeInfo);
            }

            return routeInfo;
        }
    }
}
