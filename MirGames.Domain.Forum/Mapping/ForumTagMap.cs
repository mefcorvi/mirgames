namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class ForumTagMap : EntityTypeConfiguration<ForumTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTagMap"/> class.
        /// </summary>
        public ForumTagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TagText).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("forum_tag");
            this.Property(t => t.Id).HasColumnName("tag_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.TagText).HasColumnName("tag_text");
        }
    }
}