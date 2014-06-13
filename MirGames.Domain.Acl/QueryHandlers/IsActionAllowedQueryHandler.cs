// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IsActionAllowedQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.QueryHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Entities;
    using MirGames.Domain.Acl.Public.Queries;
    using MirGames.Domain.Acl.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Queries;

    internal sealed class IsActionAllowedQueryHandler : SingleItemQueryHandler<IsActionAllowedQuery, bool>
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// The entity types resolver.
        /// </summary>
        private readonly IEntityTypesResolver entityTypesResolver;

        /// <summary>
        /// The action types resolver.
        /// </summary>
        private readonly IActionTypesResolver actionTypesResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsActionAllowedQueryHandler" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        /// <param name="entityTypesResolver">The entity types resolver.</param>
        /// <param name="actionTypesResolver">The action types resolver.</param>
        public IsActionAllowedQueryHandler(
            ICacheManagerFactory cacheManagerFactory,
            IEntityTypesResolver entityTypesResolver,
            IActionTypesResolver actionTypesResolver)
        {
            Contract.Requires(cacheManagerFactory != null);
            Contract.Requires(actionTypesResolver != null);
            Contract.Requires(entityTypesResolver != null);

            this.cacheManager = cacheManagerFactory.Create("Acl");
            this.entityTypesResolver = entityTypesResolver;
            this.actionTypesResolver = actionTypesResolver;
        }

        /// <inheritdoc />
        public override bool Execute(IReadContext readContext, IsActionAllowedQuery query, ClaimsPrincipal principal)
        {
            int entityTypeId = this.entityTypesResolver.GetEntityTypeId(query.EntityType);
            int actionId = this.actionTypesResolver.GetActionId(query.ActionName, entityTypeId);

            bool isAllow;
            if (this.TryGetFromCache(query, actionId, entityTypeId, out isAllow))
            {
                return isAllow;
            }

            if (query.EntityId.HasValue)
            {
                var permissions =
                    readContext.Query<Permission>()
                               .Where(
                                   p =>
                                   (p.UserId == query.UserId || p.UserId == null) && p.EntityTypeId == entityTypeId
                                   && (p.EntityId >= query.EntityId - 50 && p.EntityId <= query.EntityId + 50));

                permissions.ForEach(this.AddToCache);
            }

            if (this.TryGetFromCache(query, actionId, entityTypeId, out isAllow))
            {
                return isAllow;
            }

            var commonPermissions =
                readContext.Query<Permission>()
                           .Where(
                               p =>
                               (p.UserId == query.UserId || p.UserId == null) && (p.EntityTypeId == entityTypeId)
                               && p.EntityId == null);

            commonPermissions.ForEach(this.AddToCache);

            if (this.TryGetFromCache(query, actionId, entityTypeId, out isAllow))
            {
                return isAllow;
            }

            this.AddToCache(new Permission
            {
                ActionId = actionId,
                CreatedDate = DateTime.Now,
                EntityId = query.EntityId,
                EntityTypeId = entityTypeId,
                UserId = query.UserId,
                IsDenied = true
            });

            return false;
        }

        private bool TryGetFromCache(IsActionAllowedQuery query, int actionId, int entityTypeId, out bool isAllow)
        {
            var cacheKeys = new[]
            {
                this.GetCacheKey(query.UserId, actionId, entityTypeId, query.EntityId),
                this.GetCacheKey(query.UserId, actionId, entityTypeId, null),
                this.GetCacheKey(query.UserId, null, entityTypeId, null),
                this.GetCacheKey(null, actionId, entityTypeId, query.EntityId),
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

        private string GetCacheKey(int? userId, int? actionId, int entityTypeId, int? entityId)
        {
            return string.Format("{0}_{1}_{2}_{3}", entityTypeId, entityId, actionId, userId);
        }

        private void AddToCache(Permission permission)
        {
            string cacheKey = this.GetCacheKey(
                permission.UserId,
                permission.ActionId,
                permission.EntityTypeId,
                permission.EntityId);

            this.cacheManager.AddOrUpdate(cacheKey, !permission.IsDenied, DateTimeOffset.Now.AddMinutes(30));
        } 
    }
}
