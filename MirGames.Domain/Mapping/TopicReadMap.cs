namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The topic read map.
    /// </summary>
    public class TopicReadMap : EntityTypeConfiguration<topic_read>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicReadMap"/> class.
        /// </summary>
        public TopicReadMap()
        {
            // Primary Key
            this.HasKey(t => new { t.topic_id, t.user_id, t.date_read, t.comment_count_last, t.comment_id_last });

            // Properties
            this.Property(t => t.topic_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.comment_count_last).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.comment_id_last).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_topic_read", "social");
            this.Property(t => t.topic_id).HasColumnName("topic_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.date_read).HasColumnName("date_read");
            this.Property(t => t.comment_count_last).HasColumnName("comment_count_last");
            this.Property(t => t.comment_id_last).HasColumnName("comment_id_last");

            // Relationships
            this.HasRequired(t => t.topic).WithMany(t => t.topic_read).HasForeignKey(d => d.topic_id);
            this.HasRequired(t => t.user).WithMany(t => t.topic_read).HasForeignKey(d => d.user_id);
        }
    }
}