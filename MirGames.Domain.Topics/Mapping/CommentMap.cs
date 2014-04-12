// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommentMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// Mapping of the comment map.
    /// </summary>
    internal class CommentMap : EntityTypeConfiguration<Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentMap"/> class.
        /// </summary>
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            this.Property(t => t.Text)
                .IsOptional()
                .HasMaxLength(65535);

            this.Property(t => t.SourceText)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.UserLogin)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UserIP)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("prefix_comment");
            this.Property(t => t.CommentId).HasColumnName("comment_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UserLogin).HasColumnName("user_login");
            this.Property(t => t.Text).HasColumnName("comment_text");
            this.Property(t => t.SourceText).HasColumnName("comment_source_text");
            this.Property(t => t.Date).HasColumnName("comment_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.UserIP).HasColumnName("comment_user_ip");
            this.Property(t => t.Rating).HasColumnName("comment_rating");

            this.HasRequired(t => t.Topic).WithMany().HasForeignKey(t => t.TopicId);
        }
    }
}
