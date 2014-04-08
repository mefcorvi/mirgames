namespace MirGames.Infrastructure.Security
{
    /// <summary>
    /// Provides the hash string of text.
    /// </summary>
    public interface ITextHashProvider
    {
        /// <summary>
        /// Gets the hash of the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The hash of the text.</returns>
        string GetHash(string text);
    }
}