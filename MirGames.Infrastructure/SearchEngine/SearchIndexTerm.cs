// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SearchIndexTerm.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.SearchEngine
{
    /// <summary>
    /// The search index term.
    /// </summary>
    public class SearchIndexTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndexTerm"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public SearchIndexTerm(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.IsIndexed = false;
            this.IsNormalized = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is normalized.
        /// </summary>
        public bool IsNormalized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexable.
        /// </summary>
        public bool IsIndexed { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; private set; }
    }
}