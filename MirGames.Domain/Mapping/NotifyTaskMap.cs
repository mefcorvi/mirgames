namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The notify task map.
    /// </summary>
    public class NotifyTaskMap : EntityTypeConfiguration<notify_task>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyTaskMap"/> class.
        /// </summary>
        public NotifyTaskMap()
        {
            // Primary Key
            this.HasKey(t => t.notify_task_id);

            // Properties
            this.Property(t => t.user_login).HasMaxLength(30);

            this.Property(t => t.user_mail).HasMaxLength(50);

            this.Property(t => t.notify_subject).HasMaxLength(200);

            this.Property(t => t.notify_text).HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("prefix_notify_task", "social");
            this.Property(t => t.notify_task_id).HasColumnName("notify_task_id");
            this.Property(t => t.user_login).HasColumnName("user_login");
            this.Property(t => t.user_mail).HasColumnName("user_mail");
            this.Property(t => t.notify_subject).HasColumnName("notify_subject");
            this.Property(t => t.notify_text).HasColumnName("notify_text");
            this.Property(t => t.date_created).HasColumnName("date_created");
            this.Property(t => t.notify_task_status).HasColumnName("notify_task_status");
        }

        #endregion
    }
}