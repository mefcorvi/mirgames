namespace System.Web.Mvc
{
    using System.Runtime.Caching;
    using System.Text;
    using System.Web.Routing;

    public static class T4MvcCachedExtensions
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private static readonly MemoryCache Cache = new MemoryCache("T4MVC");

        /// <summary>
        /// Returns the action url.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="result">The result.</param>
        /// <returns>The url.</returns>
        public static string ActionCached(this UrlHelper urlHelper, ActionResult result)
        {
            var cacheKey = GetCacheKey(result);
            var url = (string)Cache.Get(cacheKey);
            
            if (url == null)
            {
                url = urlHelper.Action(result);
                Cache.Add(cacheKey, url, DateTimeOffset.UtcNow.AddDays(1));
            }

            return url;
        }

        private static string GetCacheKey(ActionResult actionResult)
        {
            var sb = new StringBuilder();

            foreach (var pair in actionResult.GetRouteValueDictionary())
            {
                sb.AppendFormat("|{0}={1}|", pair.Key, pair.Value);
            }

            return sb.ToString();
        }
    }
}