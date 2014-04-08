namespace MirGames.Domain.TextTransform
{
    /// <summary>
    /// The text transform rule.
    /// </summary>
    public interface ITextTransform
    {
        /// <summary>
        /// Transforms the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The specified text.</returns>
        string Transform(string text);
    }
}
