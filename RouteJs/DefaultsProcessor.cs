namespace RouteJs
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Handles parsing of optional parameters from the defaults in MVC routes
    /// </summary>
    public class DefaultsProcessor : IDefaultsProcessor
    {
        /// <summary>
        /// Name of the data token used to store area
        /// </summary>
        private const string AreaToken = "area";

        /// <summary>
        /// The route value dictionary key.
        /// </summary>
        private const string RouteValueDictionaryKey = "RouteValueDictionary";

        /// <summary>
        /// Process the defaults of the specified route
        /// </summary>
        /// <param name="route">Route to process</param>
        /// <param name="routeInfo">Output information about the route</param>
        public void ProcessDefaults(Route route, RouteInfo routeInfo)
        {
            routeInfo.Defaults = new RouteValueDictionary();
            routeInfo.Optional = new List<string>();

            if (route.Defaults == null)
            {
                return;
            }

            foreach (var kvp in route.Defaults)
            {
                if (kvp.Value == UrlParameter.Optional)
                {
                    routeInfo.Optional.Add(kvp.Key);
                }
                else if (this.ShouldAddDefault(kvp.Key))
                {
                    routeInfo.Defaults.Add(kvp.Key, kvp.Value);
                }
            }

            // Add area if it's specified in the route
            if (route.DataTokens != null && route.DataTokens.ContainsKey(AreaToken))
            {
                routeInfo.Defaults.Add("area", route.DataTokens[AreaToken]);
            }
        }

        /// <summary>
        /// Determines whether the specified default value should be added to the output
        /// </summary>
        /// <param name="key">Key of the default value</param>
        /// <returns><c>true</c> if the default value should be used, or <c>false</c> if it should
        /// be ignored.</returns>
        private bool ShouldAddDefault(string key)
        {
            // T4MVC adds RouteValueDictionary to defaults, but we don't need it.
            return key != RouteValueDictionaryKey;
        }
    }
}
