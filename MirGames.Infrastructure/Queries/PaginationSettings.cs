namespace MirGames.Infrastructure.Queries
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The pagination settings.
    /// </summary>
    public class PaginationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationSettings"/> class.
        /// </summary>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public PaginationSettings(int pageNum, int pageSize)
        {
            this.PageNum = pageNum;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNum { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the item page.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The item page.</returns>
        public static int GetItemPage(int itemIndex, int pageSize)
        {
            Contract.Requires(pageSize > 0);
            return (int)Math.Ceiling(itemIndex / (double)pageSize) - 1;
        }
    }
}