namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The user note map.
    /// </summary>
    public class UserNoteMap : EntityTypeConfiguration<user_note>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNoteMap"/> class.
        /// </summary>
        public UserNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.text).IsRequired().HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("prefix_user_note", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.target_user_id).HasColumnName("target_user_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.text).HasColumnName("text");
            this.Property(t => t.date_add).HasColumnName("date_add");

            // Relationships
            this.HasRequired(t => t.user).WithMany().HasForeignKey(d => d.target_user_id);
            this.HasRequired(t => t.user1).WithMany().HasForeignKey(d => d.user_id);
        }
    }
}