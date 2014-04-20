namespace MirGames.Domain.TextTransform
{
    /// <summary>
    /// Processes the text.
    /// </summary>
    public interface ITextProcessor
    {
        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The HTML.</returns>
        string GetHtml(string source);

        /// <summary>
        /// Gets the short HTML.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The short HTML.</returns>
        string GetShortHtml(string source);

        /// <summary>
        /// Gets the short text.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The short text.</returns>
        string GetShortText(string source);
    }
}
