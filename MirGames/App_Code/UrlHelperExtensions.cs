namespace System.Web.Mvc
{
    using System.Diagnostics;
    using System.Reflection;

    using MirGames;

    /// <summary>
    /// The URL Helper.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// The version.
        /// </summary>
        private static readonly string Version = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(MvcApplication)).Location).ProductVersion;

        /// <summary>
        /// Gets the URL to the stylesheet.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The URL to the stylesheet.</returns>
        public static string Stylesheet(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/public/v{1}/css/{0}", fileName, Version));
        }

        /// <summary>
        /// Gets the URL to the javascript.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The URL to the javascript.</returns>
        public static string Script(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/public/v{1}/js/{0}", fileName, Version));
        }
    }
}