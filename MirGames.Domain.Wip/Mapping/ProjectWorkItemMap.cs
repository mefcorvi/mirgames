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
            this.Property(t => t.ProjectId).HasColumnName("project_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.Description).HasColumnName("description");
            this.Property(t => t.TagsList).HasColumnName("tags_list");
            this.Property(t => t.State).HasColumnName("state");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.ItemType).HasColumnName("work_item_type");
        }
    }
}