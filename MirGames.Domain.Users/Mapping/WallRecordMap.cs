// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WallRecordMap.cs">
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
    /// The wall map.
    /// </summary>
    internal class WallRecordMap : EntityTypeConfiguration<WallRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallRecordMap"/> class.
        /// </summary>
        public WallRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LastReplyId).IsRequired().HasMaxLength(100);
            this.Property(t => t.AuthorIp).IsRequired().HasMaxLength(20);
            this.Property(t => t.Text).IsRequired().HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("prefix_wall");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.ParentId).HasColumnName("pid");
            this.Property(t => t.WallUserId).HasColumnName("wall_user_id");
            this.Property(t => t.AuthorId).HasColumnName("user_id");
            this.Property(t => t.RepliesCount).HasColumnName("count_reply");
            this.Property(t => t.LastReplyId).HasColumnName("last_reply");
            this.Property(t => t.DateAdd).HasColumnName("date_add");
            this.Property(t => t.AuthorIp).HasColumnName("ip");
            this.Property(t => t.Text).HasColumnName("text");

            // Relationships
            this.HasRequired(t => t.WallUser).WithMany().HasForeignKey(d => d.WallUserId);
            this.HasRequired(t => t.Author).WithMany().HasForeignKey(d => d.AuthorId);
        }
    }
}