// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SetPermissionCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Entities;
    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Acl.Public.Events;
    using MirGames.Domain.Acl.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Sets the permission.
    /// </summary>
    internal sealed class SetPermissionCommandHandler : CommandHandler<SetPermissionCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The entity types resolver.
        /// </summary>
        private readonly IEntityTypesResolver entityTypesResolver;

        /// <summary>
        /// The action types resolver.
        /// </summary>
        private readonly IActionTypesResolver actionTypesResolver;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPermissionCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="entityTypesResolver">The entity types resolver.</param>
        /// <param name="actionTypesResolver">The action types resolver.</param>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public SetPermissionCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEntityTypesResolver entityTypesResolver,
            IActionTypesResolver actionTypesResolver,
            ICacheManagerFactory cacheManagerFactory,
            IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(entityTypesResolver != null);
            Contract.Requires(actionTypesResolver != null);
            Contract.Requires(cacheManagerFactory != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.entityTypesResolver = entityTypesResolver;
            this.actionTypesResolver = actionTypesResolver;
            this.cacheManager = cacheManagerFactory.Create("Acl");
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(SetPermissionCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int entityTypeId = this.entityTypesResolver.GetEntityTypeId(command.EntityType);

            command.Actions.ForEach(action => this.SetActionPermission(command, action, entityTypeId));

            this.eventBus.Raise(new PermissionChangedEvent
            {
                Actions = command.Actions,
                EntityId = command.EntityId,
                EntityType = command.EntityType,
                IsDenied = command.IsDenied,
                UserId = command.UserId
            });
        }

        /// <summary>
        /// Sets the action permission.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="action">The action.</param>
        /// <param name="entityTypeId">The entity type identifier.</param>
        private void SetActionPermission(SetPermissionCommand command, string action, int entityTypeId)
        {
            int actionId = this.actionTypesResolver.GetActionId(action, entityTypeId);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var existPermission = writeContext
                    .Set<Permission>()
                    .SingleOrDefault(
                        p =>
                        p.UserId == command.UserId && p.EntityTypeId == entityTypeId && p.ActionId == actionId
                        && p.EntityId == command.EntityId);

                if (existPermission != null)
                {
                    writeContext.Set<Permission>().Remove(existPermission);
                }

                var permission = new Permission
                {
                    ActionId = actionId,
                    EntityId = command.EntityId,
                    EntityTypeId = entityTypeId,
                    IsDenied = command.IsDenied,
                    CreatedDate = DateTime.UtcNow,
                    UserId = command.UserId
                };

                writeContext.Set<Permission>().Add(permission);
                writeContext.SaveChanges();

                this.AddToCache(permission);
            }
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

        private string GetCacheKey(int? userId, int? actionId, int entityTypeId, int? entityId)
        {
            return string.Format("{0}_{1}_{2}_{3}", entityTypeId, entityId, actionId, userId);
        }
    }
}