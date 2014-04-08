namespace System.Web.Mvc
{
    using Newtonsoft.Json;

    /// <summary>
    /// The JSON helper.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Converts the specified object to the JSON.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="object">The object.</param>
        /// <returns>The JSON.</returns>
        public static IHtmlString ToJson(this HtmlHelper helper, object @object)
        {
            return new HtmlString(JsonConvert.SerializeObject(@object));
        }
    }
}