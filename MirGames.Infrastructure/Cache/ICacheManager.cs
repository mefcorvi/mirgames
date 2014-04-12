// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ICacheManager.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Cache
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Manages in-memory caching.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Adds the object to the cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="cacheItem">The cache item.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        void AddOrUpdate<T>(string key, T cacheItem, CacheItemPolicy cacheItemPolicy);

        /// <summary>
        /// Adds the object to the cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="cacheItem">The cache item.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        void AddOrUpdate<T>(string key, T cacheItem, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Gets the cached value or add the new.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <returns>The value.</returns>
        T GetOrAdd<T>(string key, Func<T> itemFactory, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Determines whether cache contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True whether cache contains an item with the specified key.</returns>
        bool Contains(string key);

        /// <summary>
        /// Gets the item by the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The item.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes the item by the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
    }
}
