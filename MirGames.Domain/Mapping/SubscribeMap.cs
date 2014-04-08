namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The subscribe map.
    /// </summary>
    public class SubscribeMap : EntityTypeConfiguration<subscribe>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeMap"/> class.
        /// </summary>
        public SubscribeMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.target_type).IsRequired().HasMaxLength(20);

            this.Property(t => t.mail).IsRequired().HasMaxLength(50);

            this.Property(t => t.ip).IsRequired().HasMaxLength(20);

            this.Property(t => t.key).HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("prefix_subscribe", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.target_type).HasColumnName("target_type");
            this.Property(t => t.target_id).HasColumnName("target_id");
            this.Property(t => t.mail).HasColumnName("mail");
            this.Property(t => t.date_add).HasColumnName("date_add");
            this.Property(t => t.date_remove).HasColumnName("date_remove");
            this.Property(t => t.ip).HasColumnName("ip");
            this.Property(t => t.key).HasColumnName("key");
            this.Property(t => t.status).HasColumnName("status");
        }
    }
}