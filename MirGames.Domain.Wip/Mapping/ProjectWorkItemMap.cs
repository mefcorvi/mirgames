// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectWorkItemMap.cs">
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

    internal class ProjectWorkItemMap : EntityTypeConfiguration<ProjectWorkItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectWorkItemMap"/> class.
        /// </summary>
        public ProjectWorkItemMap()
        {
            this.HasKey(t => t.WorkItemId);
            this.ToTable("wip_work_items");

            this.Property(t => t.WorkItemId).HasColumnName("work_item_id");
            this.Property(t => t.InternalId).HasColumnName("internal_id");
            this.Property(t => t.ProjectId).HasColumnName("project_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.Description).HasColumnName("description");
            this.Property(t => t.TagsList).HasColumnName("tags_list");
            this.Property(t => t.State).HasColumnName("state");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.ItemType).HasColumnName("work_item_type");
            this.Property(t => t.StartDate).HasColumnName("start_date");
            this.Property(t => t.EndDate).HasColumnName("end_date");
            this.Property(t => t.DurationInSeconds).HasColumnName("duration");
            this.Property(t => t.ParentId).HasColumnName("parent_work_item_id");
            this.Property(t => t.AuthorId).HasColumnName("author_id");

            this.Ignore(t => t.Duration);
        }
    }
}