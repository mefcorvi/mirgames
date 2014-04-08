namespace MirGames.Infrastructure.Notifications
{
    /// <summary>
    /// Processes the template.
    /// </summary>
    public interface ITemplateProcessor
    {
        /// <summary>
        /// Processes the specified template.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="template">The template.</param>
        /// <param name="model">The model.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns>The result of processing.</returns>
        string Process<TModel>(string template, TModel model, string cacheName);
    }
}
