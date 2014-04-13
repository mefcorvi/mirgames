namespace MirGames.Domain.Wip.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Wip.Entities;

    /// <summary>
    /// The project work item tag map.
    /// </summary>
    internal class ProjectWorkItemTagMap : EntityTypeConfiguration<ProjectWorkItemTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectWorkItemTagMap"/> class.
        /// </summary>
        public ProjectWorkItemTagMap()
        {
            this.HasKey(t => t.Id);
            this.ToTable("wip_work_item_tags");

            this.Property(t => t.Id).HasColumnName("tag_id");
            this.Property(t => t.WorkItemId).HasColumnName("work_item_id");
            this.Property(t => t.TagText).HasColumnName("tag_text").IsRequired().HasMaxLength(50);
        }
    }
}