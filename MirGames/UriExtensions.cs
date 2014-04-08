namespace System.Web.Mvc
{
    using System;
    using MirGames.Infrastructure;

    /// <summary>
    /// The URL extensions.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Determines whether the specified URI matches the route.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>True whether the specified URI matches the route.</returns>
        public static bool IsRouteMatch(this Uri uri, string controllerName, string actionName)
        {
            var routeInfo = new RouteInfo(uri);
            object routeController = routeInfo.RouteData.Values["controller"] ?? string.Empty;
            object routeAction = routeInfo.RouteData.Values["action"] ?? string.Empty;

            return controllerName.EqualsIgnoreCase(routeController.ToString())
                   && actionName.EqualsIgnoreCase(routeAction.ToString());
        }

        /// <summary>
        /// Gets the route parameter value.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The route parameter value.</returns>
        public static string GetRouteParameterValue(this Uri uri, string parameterName)
        {
            var routeInfo = new RouteInfo(uri);

            if (routeInfo.RouteData == null || routeInfo.RouteData.Values == null)
            {
                return null;
            }

            return routeInfo.RouteData.Values[parameterName] != null ? routeInfo.RouteData.Values[parameterName].ToString() : null;
        }
    }
}