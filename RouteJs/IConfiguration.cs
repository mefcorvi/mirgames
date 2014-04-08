namespace RouteJs
{
    /// <summary>
    /// Route JS configuration
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether to expose all routes to the site. 
        /// If <c>true</c>, all routes will be exposed unless explicitly hidden using <see cref="HideRoutesInJavaScriptAttribute"/>.
        /// If <c>false</c>, all routes will be hidden unless explicitly exposed using <see cref="ExposeRoutesInJavaScriptAttribute"/>.
        /// </summary>
        bool ExposeAllRoutes { get; }
    }
}