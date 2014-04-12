// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PaginationSettings.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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