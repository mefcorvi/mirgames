namespace MirGames.Models
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The pagination view model.
    /// </summary>
    public sealed class PaginationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationViewModel" /> class.
        /// </summary>
        /// <param name="paginationSettings">The pagination settings.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <param name="urlFactory">The URL factory.</param>
        public PaginationViewModel(PaginationSettings paginationSettings, int itemsCount, Func<int, string> urlFactory)
        {
            Contract.Requires(paginationSettings.PageNum + 1 > 0);
            Contract.Requires(paginationSettings.PageSize > 0);
            Contract.Requires(urlFactory != null);

            this.PageNumber = paginationSettings.PageNum + 1;
            this.PageSize = paginationSettings.PageSize;
            this.ItemsCount = itemsCount;
            this.UrlFactory = urlFactory;
            this.ShowPrevNextNavigation = true;
            this.HightlightCurrentPage = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether previous next navigation should be shown.
        /// </summary>
        public bool ShowPrevNextNavigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current page should be highlighted.
        /// </summary>
        public bool HightlightCurrentPage { get; set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the pages count.
        /// </summary>
        public int PagesCount
        {
            get
            {
                if (this.ItemsCount == 0)
                {
                    return 1;
                }

                return (int)Math.Max(1, Math.Ceiling(this.ItemsCount / (double)this.PageSize));
            }
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        public int ItemsCount { get; private set; }

        /// <summary>
        /// Gets the left boundary.
        /// </summary>
        public int LeftBoundary
        {
            get { return Math.Max(1, this.PageNumber - 4); }
        }

        /// <summary>
        /// Gets the right boundary.
        /// </summary>
        public int RightBoundary
        {
            get { return Math.Min(this.PagesCount, this.PageNumber + 4); }
        }

        /// <summary>
        /// Gets a value indicating whether control has previous page.
        /// </summary>
        public bool HasPreviousPage
        {
            get { return this.PageNumber > 1; }
        }

        /// <summary>
        /// Gets a value indicating whether control has next page.
        /// </summary>
        public bool HasNextPage
        {
            get { return this.PageNumber < this.PagesCount; }
        }

        /// <summary>
        /// Gets the URL factory.
        /// </summary>
        public Func<int, string> UrlFactory { get; private set; }
    }
}