// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PrincipalCache.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Security
{
    using System;
    using System.Security.Claims;

    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Stores principal in cache.
    /// </summary>
    internal sealed class PrincipalCache : IPrincipalCache
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalCache" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public PrincipalCache(ICacheManagerFactory cacheManagerFactory)
        {
            this.cacheManager = cacheManagerFactory.Create("Principals");
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetOrAdd(string sessionId, Func<ClaimsPrincipal> principalFactory)
        {
            return this.cacheManager.GetOrAdd(this.GetKey(sessionId), principalFactory, DateTimeOffset.Now.AddMinutes(5));
        }

        /// <inheritdoc />
        public void Remove(string sessionId)
        {
            this.cacheManager.Remove(this.GetKey(sessionId));
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.cacheManager.Clear();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns>The cache key.</returns>
        private string GetKey(string sessionId)
        {
            return string.Format("StringPrincipal{0}", sessionId);
        }
    }
}