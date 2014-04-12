// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PasswordRestoreRequestMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The password restore request map.
    /// </summary>
    internal class PasswordRestoreRequestMap : EntityTypeConfiguration<PasswordRestoreRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRestoreRequestMap"/> class.
        /// </summary>
        public PasswordRestoreRequestMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_password_restore");
            this.Property(t => t.CreationDate).HasColumnName("creation_date");
            this.Property(t => t.Id).HasColumnName("password_restore_id");
            this.Property(t => t.NewPassword).HasColumnName("new_password").IsRequired().HasMaxLength(255);
            this.Property(t => t.SecretKey).HasColumnName("secret_key").IsRequired().HasMaxLength(255);
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}