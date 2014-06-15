// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IPermissionsCacheManager.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Services
{
    using System;

    using MirGames.Domain.Acl.Entities;
    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Permissions cache manager.
    /// </summary>
    internal interface IPermissionsCacheManager
    {
        /// <summary>
        /// Adds to the cache.
        /// </summary>
        /// <param name="permission">The permission.</param>
        void AddToCache(Permission permission);

        /// <summary>
        /// Tries the get from cache.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="isAllow">if set to <c>true</c> [is allow].</param>
        /// <returns>
        /// True whether permission have been resolved from the cache.
        /// </returns>
        bool TryGetFromCache(int userId, int actionId, int entityTypeId, int? entityId, out bool isAllow);
    }

    internal sealed class PermissionsCacheManager : IPermissionsCacheManager
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsCacheManager"/> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public PermissionsCacheManager(ICacheManagerFactory cacheManagerFactory)
        {
            this.cacheManager = cacheManagerFactory.Create("Acl");
        }

        /// <inheritdoc />
        public void AddToCache(Permission permission)
        {
            string cacheKey = this.GetCacheKey(
                permission.UserId,
                permission.ActionId,
                permission.EntityTypeId,
                permission.EntityId);

            this.cacheManager.AddOrUpdate(cacheKey, !permission.IsDenied, this.GetExpirationDate(permission));
        }

        /// <inheritdoc />
        public bool TryGetFromCache(int userId, int actionId, int entityTypeId, int? entityId, out bool isAllow)
        {
            var cacheKeys = new[]
            {
                this.GetCacheKey(userId, actionId, entityTypeId, entityId),
                this.GetCacheKey(userId, actionId, entityTypeId, null),
                this.GetCacheKey(userId, null, entityTypeId, null),
                this.GetCacheKey(null, actionId, entityTypeId, entityId),
                this.GetCacheKey(null, actionId, entityTypeId, null),
                this.GetCacheKey(null, null, entityTypeId, null)
            };

            foreach (string cacheKey in cacheKeys)
            {
                if (this.cacheManager.Contains(cacheKey))
                {
                    {
                        isAllow = this.cacheManager.Get<bool>(cacheKey);
                        return true;
                    }
                }
            }

            isAllow = default(bool);
            return false;
        }

        /// <summary>
        /// Gets the cache expiration date.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>The cache expiration date.</returns>
        private DateTimeOffset GetExpirationDate(Permission permission)
        {
            var defaultOffset = DateTimeOffset.UtcNow.AddMinutes(30);

            if (permission.ExpirationDate == null)
            {
                return defaultOffset;
            }

            var expirationOffset = new DateTimeOffset(permission.ExpirationDate.Value);

            return expirationOffset > defaultOffset ? defaultOffset : expirationOffset;
        }

        private string GetCacheKey(int? userId, int? actionId, int entityTypeId, int? entityId)
        {
            return string.Format("{0}_{1}_{2}_{3}", entityTypeId, entityId, actionId, userId);
        }
    }
}
