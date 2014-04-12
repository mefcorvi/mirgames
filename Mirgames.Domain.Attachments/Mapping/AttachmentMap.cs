// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AttachmentMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Attachments.Entities;

    /// <summary>
    /// The user_administrator map.
    /// </summary>
    internal sealed class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentMap"/> class.
        /// </summary>
        public AttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.AttachmentId);

            // Properties
            this.Property(t => t.AttachmentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("attachments");
            this.Property(t => t.AttachmentId).HasColumnName("attachment_id").IsRequired();
            this.Property(t => t.AttachmentType).HasColumnName("type").IsRequired();
            this.Property(t => t.CreatedDate).HasColumnName("created_date").IsRequired();
            this.Property(t => t.EntityId).HasColumnName("entity_id").IsOptional();
            this.Property(t => t.EntityType).HasColumnName("entity_type").IsOptional();
            this.Property(t => t.FileName).HasColumnName("file_name").IsRequired();
            this.Property(t => t.FilePath).HasColumnName("file_path").IsRequired();
            this.Property(t => t.FileSize).HasColumnName("file_size").IsRequired();
            this.Property(t => t.IsPublished).HasColumnName("is_published").IsRequired();
            this.Property(t => t.UserId).HasColumnName("user_id").IsOptional();
        }
    }
}