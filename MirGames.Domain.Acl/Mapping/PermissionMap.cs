// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PermissionMap.cs">
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
    /// Mapping of the project Permission.
    /// </summary>
    internal class PermissionMap : EntityTypeConfiguration<Permission>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionMap"/> class.
        /// </summary>
        public PermissionMap()
        {
            this.ToTable("acl_permissions");
            this.HasKey(t => t.PermissionId);

            this.Property(t => t.PermissionId).HasColumnName("permission_id");
            this.Property(t => t.ActionId).HasColumnName("action_id");
            this.Property(t => t.EntityId).HasColumnName("entity_id");
            this.Property(t => t.EntityTypeId).HasColumnName("entity_type_id");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.ExpirationDate).HasColumnName("expiration_date");
            this.Property(t => t.IsDenied).HasColumnName("deny");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}
