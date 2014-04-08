namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// Mapping of stream subscribe entity.
    /// </summary>
    internal sealed class StreamSubscribeMap : EntityTypeConfiguration<stream_subscribe>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamSubscribeMap"/> class.
        /// </summary>
        public StreamSubscribeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.user_id, t.target_user_id });

            // Properties
            this.Property(t => t.user_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.target_user_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_stream_subscribe", "social");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.target_user_id).HasColumnName("target_user_id");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.stream_subscribe)
                .HasForeignKey(d => d.user_id);
        }
    }
}
