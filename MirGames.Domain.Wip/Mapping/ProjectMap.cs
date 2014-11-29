// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Wip.Entities;

    /// <summary>
    /// Mapping of the project entity.
    /// </summary>
    internal class ProjectMap : EntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMap"/> class.
        /// </summary>
        public ProjectMap()
        {
            this.ToTable("wip_projects");
            this.HasKey(t => t.ProjectId);

            this.Property(t => t.ProjectId).HasColumnName("project_id");
            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.Alias).HasColumnName("alias").IsRequired();
            this.Property(t => t.CreationDate).HasColumnName("creation_date");
            this.Property(t => t.Description).HasColumnName("description").IsRequired();
            this.Property(t => t.FollowersCount).HasColumnName("followers_count");
            this.Property(t => t.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
            this.Property(t => t.LastCommitMessage).HasColumnName("last_commit_message").HasMaxLength(255);
            this.Property(t => t.TagsList).HasColumnName("tags_list").IsRequired().HasMaxLength(1024);
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.Version).HasColumnName("version").IsOptional().HasMaxLength(255);
            this.Property(t => t.Votes).HasColumnName("votes");
            this.Property(t => t.VotesCount).HasColumnName("votes_count");
            this.Property(t => t.RepositoryId).HasColumnName("repository_id").IsOptional();
            this.Property(t => t.RepositoryType).HasColumnName("repository_type").IsOptional();
            this.Property(t => t.BlogId).HasColumnName("blog_id").IsOptional();
            this.Property(t => t.Genre).HasColumnName("genre").IsOptional();
            this.Property(t => t.IsSiteEnabled).HasColumnName("is_site_enabled");
            this.Property(t => t.ShortDescription).HasColumnName("short_description").IsRequired().HasMaxLength(1024);
        }
    }
}
