// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ItemNotFoundException.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Exceptions
{
    using System;

    /// <summary>
    /// Exception raised when item have not been found.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="itemId">The topic id.</param>
        public ItemNotFoundException(string type, object itemId) : base("Item " + type + "#" + itemId + " have not been found")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="itemId">The topic id.</param>
        /// <param name="innerException">The inner exception.</param>
        public ItemNotFoundException(string type, object itemId, Exception innerException)
            : base("Item " + type + "#" + itemId + " have not been found", innerException)
        {
        }

    }
}