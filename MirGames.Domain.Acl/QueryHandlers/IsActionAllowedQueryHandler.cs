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
    using MirGames.Domain.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class IsActionAllowedQueryHandler : SingleItemQueryHandler<IsActionAllowedQuery, bool>
    {
        /// <summary>
        /// The entity types resolver.
        /// </summary>
        private readonly IEntityTypesResolver entityTypesResolver;

        /// <summary>
        /// The action types resolver.
        /// </summary>
        private readonly IActionTypesResolver actionTypesResolver;

        /// <summary>
        /// The permissions cache manager.
        /// </summary>
        private readonly IPermissionsCacheManager permissionsCacheManager;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsActionAllowedQueryHandler" /> class.
        /// </summary>
        /// <param name="entityTypesResolver">The entity types resolver.</param>
        /// <param name="actionTypesResolver">The action types resolver.</param>
        /// <param name="permissionsCacheManager">The permissions cache manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public IsActionAllowedQueryHandler(
            IEntityTypesResolver entityTypesResolver,
            IActionTypesResolver actionTypesResolver,
            IPermissionsCacheManager permissionsCacheManager,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(actionTypesResolver != null);
            Contract.Requires(entityTypesResolver != null);
            Contract.Requires(permissionsCacheManager != null);
            Contract.Requires(readContextFactory != null);

            this.entityTypesResolver = entityTypesResolver;
            this.actionTypesResolver = actionTypesResolver;
            this.permissionsCacheManager = permissionsCacheManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override bool Execute(IsActionAllowedQuery query, ClaimsPrincipal principal)
        {
            if (principal.IsInRole("Administrator"))
            {
                return true;
            }

            int entityTypeId = this.entityTypesResolver.GetEntityTypeId(query.EntityType);
            int actionId = this.actionTypesResolver.GetActionId(query.ActionName, entityTypeId);

            bool isAllow;
            if (this.permissionsCacheManager.TryGetFromCache(query.UserId, actionId, entityTypeId, query.EntityId, out isAllow))
            {
                return isAllow;
            }

            using (var readContext = this.readContextFactory.Create())
            {
                var permissions = readContext.Query<Permission>()
                                         .Where(
                                             p =>
                                             (p.ExpirationDate >= DateTime.UtcNow || p.ExpirationDate == null)
                                             && (p.UserId == query.UserId || p.UserId == null)
                                             && p.EntityTypeId == entityTypeId);

                if (query.EntityId.HasValue)
                {
                    permissions =
                        permissions.Where(
                            p =>
                            (p.EntityId >= query.EntityId - 50 && p.EntityId <= query.EntityId + 50)
                            || p.EntityId == null);
                }
                else
                {
                    permissions = permissions.Where(p => p.EntityId == null);
                }

                permissions.ForEach(this.permissionsCacheManager.AddToCache);
            }

            if (this.permissionsCacheManager.TryGetFromCache(query.UserId, actionId, entityTypeId, query.EntityId, out isAllow))
            {
                return isAllow;
            }

            this.permissionsCacheManager.AddToCache(new Permission
            {
                ActionId = actionId,
                CreatedDate = DateTime.UtcNow,
                EntityId = query.EntityId,
                EntityTypeId = entityTypeId,
                UserId = query.UserId,
                IsDenied = true,
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            });

            return false;
        }
    }
}
