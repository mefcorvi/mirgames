// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IDictionaryEntityResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// Resolves entities by the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal interface IDictionaryEntityResolver<in TKey, out TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the entities.
        /// </summary>
        IEnumerable<TEntity> Entities { get; }

        /// <summary>
        /// Resolves the entity by the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Set of the entities.</returns>
        IEnumerable<TEntity> Resolve(TKey key);
    }
}