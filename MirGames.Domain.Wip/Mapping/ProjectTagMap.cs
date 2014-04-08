namespace MirGames.Domain.Wip.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Wip.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class ProjectTagMap : EntityTypeConfiguration<ProjectTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTagMap"/> class.
        /// </summary>
        public ProjectTagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TagText).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("wip_tags");
            this.Property(t => t.Id).HasColumnName("tag_id");
            this.Property(t => t.ProjectId).HasColumnName("project_id");
            this.Property(t => t.TagText).HasColumnName("tag_text");
        }
    }
}