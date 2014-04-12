// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CacheManager.cs">
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
    using System.Diagnostics.Contracts;
    using System.Runtime.Caching;

    /// <summary>
    /// Manages in-memory cache.
    /// </summary>
    internal sealed class CacheManager : ICacheManager
    {
        /// <summary>
        /// The null value
        /// </summary>
        private static readonly object NullValue = new object();

        /// <summary>
        /// The memory cache.
        /// </summary>
        private readonly MemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        public CacheManager()
        {
            this.memoryCache = MemoryCache.Default;
        }

        /// <inheritdoc />
        public void AddOrUpdate<T>(string key, T cacheItem, CacheItemPolicy cacheItemPolicy)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));
            Contract.Requires(cacheItemPolicy != null);

            this.memoryCache.Set(key, cacheItem, cacheItemPolicy);
        }

        /// <inheritdoc />
        public void AddOrUpdate<T>(string key, T cacheItem, DateTimeOffset absoluteExpiration)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));

            this.memoryCache.Set(key, cacheItem, absoluteExpiration);
        }

        /// <inheritdoc />
        public T GetOrAdd<T>(string key, Func<T> itemFactory, DateTimeOffset absoluteExpiration)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));
            Contract.Requires(itemFactory != null);

            if (this.memoryCache.Contains(key))
            {
                return this.Get<T>(key);
            }

            object newValue = itemFactory.Invoke();
            this.memoryCache.Add(key, newValue ?? NullValue, absoluteExpiration);

            return (T)newValue;
        }

        /// <inheritdoc />
        public bool Contains(string key)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));

            return this.memoryCache.Contains(key);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));

            var item = this.memoryCache.Get(key);
            return item == NullValue ? default(T) : (T)item;
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));
            this.memoryCache.Remove(key);
        }
    }
}