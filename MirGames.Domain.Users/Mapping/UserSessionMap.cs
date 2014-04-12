// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserSessionMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The session map.
    /// </summary>
    internal class UserSessionMap : EntityTypeConfiguration<UserSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionMap"/> class.
        /// </summary>
        public UserSessionMap()
        {
            this.ToTable("prefix_session");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("session_key").IsRequired().HasMaxLength(32);
            this.Property(t => t.CreationIP).HasColumnName("session_ip_create").IsRequired();
            this.Property(t => t.LastVisitIP).HasColumnName("session_ip_last").IsRequired();
            this.Property(t => t.CreateDate).HasColumnName("session_date_create").IsRequired();
            this.Property(t => t.LastDate).HasColumnName("session_date_last").IsRequired();
            this.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
        }
    }
}