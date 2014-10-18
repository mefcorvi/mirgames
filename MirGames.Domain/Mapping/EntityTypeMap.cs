// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityTypeMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// Mapping of the project EntityType.
    /// </summary>
    internal class EntityTypeMap : EntityTypeConfiguration<EntityType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeMap"/> class.
        /// </summary>
        public EntityTypeMap()
        {
            this.ToTable("entity_types");
            this.HasKey(t => t.EntityTypeId);

            this.Property(t => t.EntityTypeId).HasColumnName("entity_type_id");
            this.Property(t => t.TypeName).HasColumnName("entity_type");
        }
    }
}
