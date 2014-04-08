namespace System.Web.Mvc
{
    using System.Web;

    using MarkdownDeep;

    /// <summary>
    /// The markdown helper.
    /// </summary>
    public static class MarkdownHelper
    {
        /// <summary>
        /// Markdowns the specified text.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="text">The text.</param>
        /// <returns>Return processed text.</returns>
        public static IHtmlString Markdown(this HtmlHelper helper, string text)
        {
            var markdown = new Markdown
                {
                    SafeMode = true,
                    NoFollowLinks = true,
                    ExtraMode = true,
                    AutoHeadingIDs = false
                };
            return helper.Raw(markdown.Transform(text));
        }
    }
}