// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CacheManagerFactory.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;

    /// <summary>
    /// Factory of the cache managers.
    /// </summary>
    internal sealed class CacheManagerFactory : ICacheManagerFactory, IDisposable
    {
        /// <summary>
        /// The domain managers.
        /// </summary>
        private readonly IDictionary<string, CacheManager> domainManagers;

        /// <summary>
        /// The locker.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManagerFactory"/> class.
        /// </summary>
        public CacheManagerFactory()
        {
            this.domainManagers = new Dictionary<string, CacheManager>();
        }

        /// <inheritdoc />
        public ICacheManager Create(string domain)
        {
            if (!this.domainManagers.ContainsKey(domain))
            {
                lock (this.locker)
                {
                    if (!this.domainManagers.ContainsKey(domain))
                    {
                        this.domainManagers.Add(domain, new CacheManager(() => new MemoryCache(domain)));
                    }
                }
            }

            return this.domainManagers[domain];
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (this.locker)
            {
                if (this.domainManagers.Any())
                {
                    this.domainManagers.ForEach(d => this.Dispose());
                    this.domainManagers.Clear();
                }
            }
        }
    }
}