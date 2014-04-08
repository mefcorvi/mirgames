namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The topic photo map.
    /// </summary>
    public class TopicPhotoMap : EntityTypeConfiguration<topic_photo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicPhotoMap"/> class.
        /// </summary>
        public TopicPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.path).IsRequired().HasMaxLength(255);

            this.Property(t => t.description).HasMaxLength(65535);

            this.Property(t => t.target_tmp).HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("prefix_topic_photo", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.topic_id).HasColumnName("topic_id");
            this.Property(t => t.path).HasColumnName("path");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.target_tmp).HasColumnName("target_tmp");

            // Relationships
            this.HasOptional(t => t.topic).WithMany(t => t.topic_photo).HasForeignKey(d => d.topic_id);
        }
    }
}