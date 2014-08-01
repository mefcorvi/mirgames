namespace RouteJs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Caching;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Filters the rendered routes based on the <see cref="ExposeRoutesInJavaScriptAttribute"/> and 
    /// <see cref="HideRoutesInJavaScriptAttribute"/> attributes on ASP.NET MVC controllers.
    /// </summary>
    public class MvcRouteFilter : IRouteFilter
    {
        /// <summary>
        /// Route JS configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// ASP.NET routes
        /// </summary>
        private readonly RouteCollection routeCollection;

        /// <summary>
        /// A mapping of namespace prefix to area name
        /// </summary>
        private readonly IDictionary<string, string> areaNamespaceMapping;

        /// <summary>
        /// Whitelist of controllers whose routes are always rendered
        /// </summary>
        private WhiteLists whiteLists;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcRouteFilter" /> class.
        /// </summary>
        /// <param name="configuration">The RouteJS configuration.</param>
        /// <param name="routeCollection">The ASP.NET routes</param>
        public MvcRouteFilter(IConfiguration configuration, RouteCollection routeCollection)
        {
            this.configuration = configuration;
            this.routeCollection = routeCollection;

            this.areaNamespaceMapping = this.GetAreaNamespaceMap();

            if (!MemoryCache.Default.Contains("MVC.RouteLists"))
            {
                MemoryCache.Default.Add("MVC.RouteLists", this.BuildLists(), DateTimeOffset.Now.AddDays(1));
            }

            this.whiteLists = (WhiteLists)MemoryCache.Default["MVC.RouteLists"];
        }

        /// <summary>
        /// Build the controller whitelist and blacklist
        /// </summary>
        private WhiteLists BuildLists()
        {
            var lists = new WhiteLists();

            // Get all the controllers from all loaded assemblies
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(this.GetTypesFromAssembly)
                .Where(type => type.IsSubclassOf(typeof(Controller)))
                .ToList();

            // Check which ones implement the attributes
            lists.ControllerWhitelist = this.ControllersImplementingAttribute(controllers, typeof(ExposeRoutesInJavaScriptAttribute));
            lists.ControllerBlacklist = this.ControllersImplementingAttribute(controllers, typeof(HideRoutesInJavaScriptAttribute));

            // Check for exposed controllers in areas - If any controller in the area is exposed, any 
            // default routes in the area need to be exposed as well.
            lists.AreaWhitelist = new HashSet<string>();
            foreach (var controller in lists.ControllerWhitelist)
            {
                var areaKey =
                    this.areaNamespaceMapping.Keys.FirstOrDefault(
                        areaNs => controller.Value.Namespace != null && controller.Value.Namespace.StartsWith(areaNs));
                if (areaKey != null)
                {
                    lists.AreaWhitelist.Add(this.areaNamespaceMapping[areaKey]);
                }
            }

            return lists;
        }

        /// <summary>
        /// Retrieve all the types exposed by the specified assembly
        /// </summary>
        /// <param name="assembly">Assembly to scan</param>
        /// <returns>All the types exposed by the assembly</returns>
        private IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // If there was an error loading a type from the assembly, only return the types 
                // that were actually loaded successfully
                return ex.Types.Where(type => type != null);
            }
        }

        /// <summary>
        /// Gets a mapping of  namespace prefix to area name
        /// </summary>
        /// <returns>The mapping.</returns>
        private IDictionary<string, string> GetAreaNamespaceMap()
        {
            var mapping = new Dictionary<string, string>();

            foreach (var route in this.routeCollection.GetRoutes().OfType<Route>())
            {
                // Skip routes with no area or namespace
                if (route.DataTokens == null || route.DataTokens["area"] == null || route.DataTokens["Namespaces"] == null)
                {
                    continue;
                }

                var area = (string)route.DataTokens["area"];
                var namespaces = (IList<string>)route.DataTokens["Namespaces"];
                foreach (var ns in namespaces)
                {
                    // MVC adds namespaces in the format "Daniel15.Areas.Blah.*", but we don't 
                    // care about the asterisk.
                    mapping[ns.TrimEnd('*')] = area;
                }
            }

            return mapping;
        }

        /// <summary>
        /// Returns all the controllers that implement the specified attribute
        /// </summary>
        /// <param name="types">The controller types.</param>
        /// <param name="attributeType">Type of the attribute to check for.</param>
        /// <returns>All the types that implement the specified attribute</returns>
        private Dictionary<string, Type> ControllersImplementingAttribute(IEnumerable<Type> types, Type attributeType)
        {
            return types
                .Where(c => c.IsDefined(attributeType, true))
                // Remove "Controller" from the class name as it's not used when referencing the controller
                .ToDictionary(c => c.Name.Replace("Controller", string.Empty), c => c);
        }

        /// <summary>
        /// Check whether the specified route should be exposed in the JavaScript output
        /// </summary>
        /// <param name="routeBase">Route to check</param>
        /// <returns>
        ///   <c>false</c> if the route should definitely be blocked, <c>true</c> if the route should be exposed (or unsure)
        /// </returns>
        public bool AllowRoute(RouteBase routeBase)
        {
            // Allow if we don't know what it is (another filter can take care of it)
            var route = routeBase as Route;
            if (route == null)
            {
                return true;
            }

            if (route.Defaults == null)
            {
                return true;
            }

            // If there's no controller specified, we need to check if it's in an area
            if (!route.Defaults.ContainsKey("controller"))
            {
                // Not an area, so it's a "regular" default route
                if (route.DataTokens == null || !route.DataTokens.ContainsKey("area"))
                {
                    return true;
                }

                // Exposing all routes, or an area that's explicitly whitelisted
                if (this.configuration.ExposeAllRoutes || this.whiteLists.AreaWhitelist.Contains(route.DataTokens["area"]))
                {
                    return true;
                }

                // In an area that's not exposed, so this route shouldn't be exposed.
                return false;
            }

            var controller = route.Defaults["controller"].ToString();

            // If explicitly blacklisted, always deny
            if (this.whiteLists.ControllerBlacklist.Keys.Contains(controller))
            {
                return false;
            }

            // If explicitly whitelisted, always allow
            if (this.whiteLists.ControllerWhitelist.Keys.Contains(controller))
            {
                return true;
            }

            // Otherwise, allow based on configuration
            return this.configuration.ExposeAllRoutes;
        }
    }
}
