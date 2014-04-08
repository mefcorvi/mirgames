namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The reminder map.
    /// </summary>
    public class ReminderMap : EntityTypeConfiguration<reminder>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderMap"/> class.
        /// </summary>
        public ReminderMap()
        {
            // Primary Key
            this.HasKey(t => t.reminder_code);

            // Properties
            this.Property(t => t.reminder_code).IsRequired().HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("prefix_reminder", "social");
            this.Property(t => t.reminder_code).HasColumnName("reminder_code");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.reminder_date_add).HasColumnName("reminder_date_add");
            this.Property(t => t.reminder_date_used).HasColumnName("reminder_date_used");
            this.Property(t => t.reminder_date_expire).HasColumnName("reminder_date_expire");
            this.Property(t => t.reminde_is_used).HasColumnName("reminde_is_used");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.reminder).HasForeignKey(d => d.user_id);
        }

        #endregion
    }
}