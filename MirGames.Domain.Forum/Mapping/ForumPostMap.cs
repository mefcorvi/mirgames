// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The forum post mapping.
    /// </summary>
    internal sealed class ForumPostMap : EntityTypeConfiguration<ForumPost>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostMap"/> class.
        /// </summary>
        public ForumPostMap()
        {
            this.ToTable("forum_post");

            this.HasKey(t => t.PostId);

            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.AuthorLogin).HasColumnName("author_login");

            this.Property(t => t.AuthorIP)
                .IsRequired()
                .HasMaxLength(48)
                .HasColumnName("author_ip");

            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.IsHidden).HasColumnName("is_hidden");
            this.Property(t => t.PostId).HasColumnName("post_id");
            this.Property(t => t.SourceText).IsOptional().HasColumnName("source_text");
            this.Property(t => t.Text).IsRequired().HasColumnName("text");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.IsStartPost).HasColumnName("is_start_post");
            this.Property(t => t.VotesRating).HasColumnName("votes_rating");
            this.Property(t => t.VotesCount).HasColumnName("votes_count");
            
            this.HasRequired(t => t.Topic).WithMany().HasForeignKey(t => t.TopicId);
        }
    }
}