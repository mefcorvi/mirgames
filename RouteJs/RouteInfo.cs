namespace RouteJs
{
    using System.Collections.Generic;
    using System.Web.Routing;

    /// <summary>
    /// Information about a route
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// Gets or sets URL of the route
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets default values for the route
        /// </summary>
        public RouteValueDictionary Defaults { get; set; }
        
        /// <summary>
        /// Gets or sets constraints imposed on the route
        /// </summary>
        public RouteValueDictionary Constraints { get; set; }
        
        /// <summary>
        /// Gets or sets any optional parameters in the URL
        /// </summary>
        public IList<string> Optional { get; set; }
    }
}
