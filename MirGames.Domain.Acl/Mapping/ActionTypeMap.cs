// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ActionTypeMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Acl.Entities;

    /// <summary>
    /// Mapping of the project ActionType.
    /// </summary>
    internal class ActionTypeMap : EntityTypeConfiguration<ActionType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTypeMap"/> class.
        /// </summary>
        public ActionTypeMap()
        {
            this.ToTable("acl_action_types");
            this.HasKey(t => t.ActionId);

            this.Property(t => t.ActionId).HasColumnName("action_id");
            this.Property(t => t.ActionName).HasColumnName("action_name");
            this.Property(t => t.EntityTypeId).HasColumnName("entity_type_id");
        }
    }
}
