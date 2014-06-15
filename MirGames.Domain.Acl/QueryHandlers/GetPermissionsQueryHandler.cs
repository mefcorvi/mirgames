// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetPermissionsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Entities;
    using MirGames.Domain.Acl.Public.Queries;
    using MirGames.Domain.Acl.Public.ViewModels;
    using MirGames.Domain.Acl.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetPermissionsQueryHandler : QueryHandler<GetPermissionsQuery, PermissionViewModel>
    {
        /// <summary>
        /// The entity types resolver.
        /// </summary>
        private readonly IEntityTypesResolver entityTypesResolver;

        /// <summary>
        /// The action types resolver.
        /// </summary>
        private readonly IActionTypesResolver actionTypesResolver;

        public GetPermissionsQueryHandler(
            IEntityTypesResolver entityTypesResolver,
            IActionTypesResolver actionTypesResolver)
        {
            this.entityTypesResolver = entityTypesResolver;
            this.actionTypesResolver = actionTypesResolver;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetPermissionsQuery query, ClaimsPrincipal principal)
        {
            return this.GetPermissions(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<PermissionViewModel> Execute(
            IReadContext readContext,
            GetPermissionsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            return
                this.ApplyPagination(this.GetPermissions(readContext, query), pagination)
                    .Select(p => new PermissionViewModel
                    {
                        ActionId = p.ActionId,
                        CreatedDate = p.CreatedDate,
                        EntityId = p.EntityId,
                        EntityTypeId = p.EntityTypeId,
                        ExpirationDate = p.ExpirationDate,
                        IsDenied = p.IsDenied,
                        PermissionId = p.PermissionId,
                        UserId = p.UserId
                    })
                    .ToList();
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The query of permissions.</returns>
        private IQueryable<Permission> GetPermissions(IReadContext readContext, GetPermissionsQuery query)
        {
            int? entityTypeId = !query.EntityType.IsNullOrEmpty()
                                    ? this.entityTypesResolver.GetEntityTypeId(query.EntityType)
                                    : (int?)null;

            int? actionId = (!string.IsNullOrEmpty(query.ActionName) && entityTypeId.HasValue)
                                ? this.actionTypesResolver.GetActionId(query.ActionName, entityTypeId.Value)
                                : (int?)null;

            IQueryable<Permission> permissions = readContext.Query<Permission>();

            if (query.FilterByActionName)
            {
                permissions = permissions.Where(p => p.ActionId == actionId);
            }

            if (query.FilterByEntityId)
            {
                permissions = permissions.Where(p => p.EntityId == query.EntityId);
            }

            if (query.FilterByEntityType)
            {
                permissions = permissions.Where(p => p.EntityTypeId == entityTypeId);
            }

            if (query.FilterByUserId)
            {
                permissions = permissions.Where(p => p.UserId == query.UserId);
            }

            return permissions;
        }
    }
}
