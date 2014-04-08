namespace MirGames.Infrastructure.Utilities
{
    /// <summary>
    /// The content type provider.
    /// </summary>
    public interface IContentTypeProvider
    {
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>The content type.</returns>
        string GetContentType(string fileExtension);
    }
}
