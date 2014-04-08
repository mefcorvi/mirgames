namespace MirGames.Infrastructure.Notifications
{
    /// <summary>
    /// Processes the razor templates.
    /// </summary>
    internal sealed class RazorTemplateProcessor : ITemplateProcessor
    {
        /// <inheritdoc />
        public string Process<TModel>(string template, TModel model, string cacheName)
        {
            return RazorEngine.Razor.Parse(template, model, cacheName);
        }
    }
}
