// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CacheManager.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
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
    internal sealed class CacheManager : ICacheManager, IDisposable
    {
        /// <summary>
        /// The null value
        /// </summary>
        private static readonly object NullValue = new object();

        /// <summary>
        /// The memory cache factory.
        /// </summary>
        private readonly Func<MemoryCache> memoryCacheFactory;

        /// <summary>
        /// The memory cache.
        /// </summary>
        private MemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager" /> class.
        /// </summary>
        /// <param name="memoryCacheFactory">The memory cache factory.</param>
        public CacheManager(Func<MemoryCache> memoryCacheFactory)
        {
            Contract.Requires(memoryCacheFactory != null);

            this.memoryCacheFactory = memoryCacheFactory;
            this.memoryCache = this.memoryCacheFactory.Invoke();
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

            var lazyFactory = new Lazy<T>(itemFactory);
            var value = (Lazy<T>)this.memoryCache.AddOrGetExisting(key, lazyFactory, absoluteExpiration);

            return (value ?? lazyFactory).Value;
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

        /// <inheritdoc />
        public void Clear()
        {
            this.memoryCache.Dispose();
            this.memoryCache = this.memoryCacheFactory.Invoke();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.memoryCache != null)
            {
                this.memoryCache.Dispose();
            }
        }
    }
}