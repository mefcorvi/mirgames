namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The talk map.
    /// </summary>
    public class TalkMap : EntityTypeConfiguration<talk>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalkMap"/> class.
        /// </summary>
        public TalkMap()
        {
            // Primary Key
            this.HasKey(t => t.talk_id);

            // Properties
            this.Property(t => t.talk_title).IsRequired().HasMaxLength(200);

            this.Property(t => t.talk_text).IsRequired().HasMaxLength(65535);

            this.Property(t => t.talk_user_ip).IsRequired().HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("prefix_talk", "social");
            this.Property(t => t.talk_id).HasColumnName("talk_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.talk_title).HasColumnName("talk_title");
            this.Property(t => t.talk_text).HasColumnName("talk_text");
            this.Property(t => t.talk_date).HasColumnName("talk_date");
            this.Property(t => t.talk_date_last).HasColumnName("talk_date_last");
            this.Property(t => t.talk_user_id_last).HasColumnName("talk_user_id_last");
            this.Property(t => t.talk_user_ip).HasColumnName("talk_user_ip");
            this.Property(t => t.talk_comment_id_last).HasColumnName("talk_comment_id_last");
            this.Property(t => t.talk_count_comment).HasColumnName("talk_count_comment");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.talk).HasForeignKey(d => d.user_id);
        }
    }
}