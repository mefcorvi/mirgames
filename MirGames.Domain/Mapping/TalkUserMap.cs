namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The talk_user map.
    /// </summary>
    public class TalkUserMap : EntityTypeConfiguration<talk_user>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalkUserMap"/> class.
        /// </summary>
        public TalkUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.talk_id, t.user_id, t.comment_id_last, t.comment_count_new });

            // Properties
            this.Property(t => t.talk_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.comment_id_last).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.comment_count_new).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_talk_user", "social");
            this.Property(t => t.talk_id).HasColumnName("talk_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.date_last).HasColumnName("date_last");
            this.Property(t => t.comment_id_last).HasColumnName("comment_id_last");
            this.Property(t => t.comment_count_new).HasColumnName("comment_count_new");
            this.Property(t => t.talk_user_active).HasColumnName("talk_user_active");

            // Relationships
            this.HasRequired(t => t.talk).WithMany(t => t.talk_user).HasForeignKey(d => d.talk_id);
            this.HasRequired(t => t.user).WithMany(t => t.talk_user).HasForeignKey(d => d.user_id);
        }
    }
}