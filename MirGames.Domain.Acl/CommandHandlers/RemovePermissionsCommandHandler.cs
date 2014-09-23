// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RemovePermissionsCommandHandler.cs">
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
    using MirGames.Domain.Acl.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class RemovePermissionsCommandHandler : CommandHandler<RemovePermissionsCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The entity types resolver.
        /// </summary>
        private readonly IDictionaryEntityResolver<string, EntityType> entityTypesResolver;

        /// <summary>
        /// The action types resolver.
        /// </summary>
        private readonly IDictionaryEntityResolver<string, ActionType> actionTypesResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovePermissionsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="entityTypesResolver">The entity types resolver.</param>
        /// <param name="actionTypesResolver">The action types resolver.</param>
        public RemovePermissionsCommandHandler(
            IWriteContextFactory writeContextFactory,
            IDictionaryEntityResolver<string, EntityType> entityTypesResolver,
            IDictionaryEntityResolver<string, ActionType> actionTypesResolver)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(entityTypesResolver != null);
            Contract.Requires(actionTypesResolver != null);

            this.writeContextFactory = writeContextFactory;
            this.entityTypesResolver = entityTypesResolver;
            this.actionTypesResolver = actionTypesResolver;
        }

        /// <inheritdoc />
        protected override void Execute(RemovePermissionsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int? entityTypeId = this.GetEntityTypeId(command);
            int? actionId = this.GetActionId(command, entityTypeId);

            if (!command.UserId.HasValue && !command.EntityId.HasValue && !entityTypeId.HasValue && !actionId.HasValue)
            {
                throw new InvalidOperationException("The whole set of permissions couldn't be deleted.");
            }

            using (var writeContext = this.writeContextFactory.Create())
            {
                IQueryable<Permission> existPermissions = writeContext.Set<Permission>();

                if (command.UserId.HasValue)
                {
                    existPermissions = existPermissions.Where(p => p.UserId == command.UserId);
                }

                if (command.EntityId.HasValue)
                {
                    if (!entityTypeId.HasValue)
                    {
                        throw new InvalidOperationException("Permission couldn't be deleted only by the entity identifier. The entity type is required.");
                    }

                    existPermissions = existPermissions.Where(p => p.EntityId == command.EntityId && p.EntityTypeId == entityTypeId);
                }

                if (entityTypeId.HasValue)
                {
                    existPermissions = existPermissions.Where(p => p.EntityTypeId == entityTypeId);
                }

                if (actionId.HasValue)
                {
                    existPermissions = existPermissions.Where(p => p.ActionId == actionId);
                }

                writeContext.Set<Permission>().RemoveRange(existPermissions);
                writeContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the action identifier.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <returns>The action identifier.</returns>
        private int? GetActionId(RemovePermissionsCommand command, int? entityTypeId)
        {
            return this.actionTypesResolver
                       .Resolve(command.ActionName)
                       .Where(a => a.EntityTypeId == entityTypeId)
                       .Select(a => (int?)a.ActionId)
                       .FirstOrDefault();
        }

        /// <summary>
        /// Gets the entity type identifier.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The entity type identifier.</returns>
        private int? GetEntityTypeId(RemovePermissionsCommand command)
        {
            return this.entityTypesResolver
                       .Resolve(command.EntityType)
                       .Select(e => (int?)e.EntityTypeId)
                       .FirstOrDefault();
        }
    }
}