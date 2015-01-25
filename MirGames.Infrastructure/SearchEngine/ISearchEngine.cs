// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ISearchEngine.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
        /// <param name="terms">The terms.</param>
        void Index(int id, string documentType, string text, params SearchIndexTerm[] terms);

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
        /// <param name="terms">The terms.</param>
        /// <returns>
        /// A set of document identifiers
        /// </returns>
        IEnumerable<SearchResult> Search(string documentType, string searchString, params SearchIndexTerm[] terms);

        /// <summary>
        /// Gets the count of documents.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="terms">The terms.</param>
        /// <returns>
        /// The count of documents.
        /// </returns>
        int GetCount(string documentType, string searchString, params SearchIndexTerm[] terms);
    }
}
