// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserAdministratorMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The user_administrator map.
    /// </summary>
    internal class UserAdministratorMap : EntityTypeConfiguration<UserAdministrator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdministratorMap"/> class.
        /// </summary>
        public UserAdministratorMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_user_administrator");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}