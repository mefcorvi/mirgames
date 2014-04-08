namespace MirGames.Domain.TextTransform
{
    /// <summary>
    /// The short text extractor.
    /// </summary>
    public interface IShortTextExtractor
    {
        /// <summary>
        /// Extracts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The short version of text.</returns>
        string Extract(string text);
    }
}