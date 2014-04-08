namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The topic_content map.
    /// </summary>
    internal class TopicContentMap : EntityTypeConfiguration<TopicContent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicContentMap"/> class.
        /// </summary>
        public TopicContentMap()
        {
            // Primary Key
            this.HasKey(t => t.TopicId);

            // Properties
            this.Property(t => t.TopicId);

            this.Property(t => t.TopicText).IsRequired().HasMaxLength(1073741823);
            this.Property(t => t.TopicTextShort).IsRequired().HasMaxLength(65535);
            this.Property(t => t.TopicTextSource).IsRequired().HasMaxLength(1073741823);
            this.Property(t => t.TopicExtra).IsRequired().HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("prefix_topic_content");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.TopicText).HasColumnName("topic_text");
            this.Property(t => t.TopicTextShort).HasColumnName("topic_text_short");
            this.Property(t => t.TopicTextSource).HasColumnName("topic_text_source");
            this.Property(t => t.TopicExtra).HasColumnName("topic_extra");
        }
    }
}