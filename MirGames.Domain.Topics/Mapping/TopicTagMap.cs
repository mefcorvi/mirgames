namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class TopicTagMap : EntityTypeConfiguration<TopicTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicTagMap"/> class.
        /// </summary>
        public TopicTagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TagText).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("topic_tag");
            this.Property(t => t.Id).HasColumnName("tag_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.TagText).HasColumnName("tag_text");
        }
    }
}