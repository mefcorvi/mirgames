namespace MirGames.Infrastructure.SearchEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// The search engine.
    /// </summary>
    public interface ISearchEngine
    {
        /// <summary>
        /// Indexes the document with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="text">The text.</param>
        void Index(int id, string documentType, string text);

        /// <summary>
        /// Removes the document with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="documentType">Type of the document.</param>
        void Remove(int id, string documentType);

        /// <summary>
        /// Clears the index.
        /// </summary>
        void ClearIndex();

        /// <summary>
        /// Searches document with the specified document type.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>A set of document identifiers</returns>
        IEnumerable<SearchResult> Search(string documentType, string searchString);

        /// <summary>
        /// Gets the count of documents.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>The count of documents.</returns>
        int GetCount(string documentType, string searchString);
    }
}
