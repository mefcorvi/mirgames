// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityTypesResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Services
{
    using System.Linq;

    using MirGames.Domain.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Repositories;

    internal sealed class EntityTypesResolver : DictionaryEntityResolver<string, EntityType>, IEntityTypesResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypesResolver"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public EntityTypesResolver(IReadContextFactory readContextFactory) : base(readContextFactory)
        {
        }

        /// <inheritdoc />
        public int? FindEntityTypeId(string entityType)
        {
            var entityTypeItem = this.Resolve(entityType).FirstOrDefault();

            if (entityTypeItem == null)
            {
                return null;
            }

            return entityTypeItem.EntityTypeId;
        }

        /// <inheritdoc />
        protected override bool IsSatisfied(EntityType entity, string key)
        {
            return entity.TypeName.EqualsIgnoreCase(key);
        }
    }
}
