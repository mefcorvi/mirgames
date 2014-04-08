namespace MirGames.Domain.Tools.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Tools.Entities;

    /// <summary>
    /// The topic tag map.
    /// </summary>
    internal class EventLogMap : EntityTypeConfiguration<EventLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogMap"/> class.
        /// </summary>
        public EventLogMap()
        {
            this.ToTable("eventLog");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.Login).HasMaxLength(255).HasColumnName("login");
            this.Property(t => t.EventLogType).IsRequired().HasColumnName("eventType");
            this.Property(t => t.Message).IsRequired().HasMaxLength(1024).HasColumnName("message");
            this.Property(t => t.Date).HasColumnName("date");
            this.Property(t => t.Details).HasColumnName("details");
        }
    }
}