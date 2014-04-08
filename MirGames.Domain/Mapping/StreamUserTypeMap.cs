namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The stream user type map.
    /// </summary>
    public class StreamUserTypeMap : EntityTypeConfiguration<stream_user_type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamUserTypeMap"/> class.
        /// </summary>
        public StreamUserTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.user_id);

            // Properties
            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.event_type).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("prefix_stream_user_type", "social");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.event_type).HasColumnName("event_type");

            // Relationships
            this.HasRequired(t => t.user).WithOptional();
        }
    }
}