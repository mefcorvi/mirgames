namespace MirGames.Infrastructure.SearchEngine
{
    /// <summary>
    /// Represents an one item of search results.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public float Score { get; set; }
    }
}