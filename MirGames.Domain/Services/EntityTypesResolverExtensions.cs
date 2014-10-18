// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityTypesResolverExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Services
{
    using MirGames.Domain.Exceptions;

    /// <summary>
    /// Extensions for entity types resolver.
    /// </summary>
    public static class EntityTypesResolverExtensions
    {
        /// <summary>
        /// Gets the entity type identifier.
        /// </summary>
        /// <param name="entityTypesResolver">The entity types resolver.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>The entity type identifier.</returns>
        /// <exception cref="ItemNotFoundException">Thrown if entity type has been found.</exception>
        public static int GetEntityTypeId(this IEntityTypesResolver entityTypesResolver, string entityType)
        {
            var entityTypeItem = entityTypesResolver.FindEntityTypeId(entityType);

            if (entityTypeItem == null)
            {
                throw new ItemNotFoundException("EntityType", entityType);
            }

            return entityTypeItem.Value;
        }
    }
}