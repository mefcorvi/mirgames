namespace RouteJs
{
    using System.Configuration;

    /// <summary>
    /// Implementation of <see cref="IConfiguration"/> using an ASP.NET configuration section.
    /// </summary>
    public class RouteJsConfigurationSection : ConfigurationSection, IConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to expose all routes to the site.
        /// If <c>true</c>, all routes will be exposed unless explicitly hidden using <see cref="HideRoutesInJavaScriptAttribute" />.
        /// If <c>false</c>, all routes will be hidden unless explicitly exposed using <see cref="ExposeRoutesInJavaScriptAttribute" />.
        /// </summary>
        [ConfigurationProperty("exposeAllRoutes", DefaultValue = true)]
        public bool ExposeAllRoutes
        {
            get { return (bool)this["exposeAllRoutes"]; }
            set { this["exposeAllRoutes"] = value; }
        }
    }
}
