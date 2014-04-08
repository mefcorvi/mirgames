namespace RouteJs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Web.Routing;

    /// <summary>
    /// Utilities for ASP.NET URL routing.
    /// </summary>
    /// <remarks>
    /// Unfortunately, the list of routes is private, so we need to use reflection to get to it :(
    /// </remarks>
    public static class RouteCollectionUtils
    {
        /// <summary>
        /// Method used to get routes from a route collection.
        /// </summary>
        private static readonly PropertyInfo GetRoutesProperty;

        /// <summary>
        /// Initializes static members of the <see cref="RouteCollectionUtils"/> class.
        /// </summary>
        static RouteCollectionUtils()
        {
            GetRoutesProperty = typeof(Collection<RouteBase>).GetProperty("Items", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Get the routes in the specified route collection.
        /// </summary>
        /// <param name="routeCollection">The route collection.</param>
        /// <returns>The list of routes.</returns>
        public static IList<RouteBase> GetRoutes(this RouteCollection routeCollection)
        {
            return (IList<RouteBase>)GetRoutesProperty.GetValue(routeCollection, null);
        }
    }
}