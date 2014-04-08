namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The stream event map.
    /// </summary>
    public class StreamEventMap : EntityTypeConfiguration<stream_event>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEventMap"/> class.
        /// </summary>
        public StreamEventMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.event_type).IsRequired().HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("prefix_stream_event", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.event_type).HasColumnName("event_type");
            this.Property(t => t.target_id).HasColumnName("target_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.date_added).HasColumnName("date_added");
            this.Property(t => t.publish).HasColumnName("publish");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.stream_event).HasForeignKey(d => d.user_id);
        }

        #endregion
    }
}