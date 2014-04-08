namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The user feed subscribe map.
    /// </summary>
    public class UserFeedSubscribeMap : EntityTypeConfiguration<userfeed_subscribe>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFeedSubscribeMap"/> class.
        /// </summary>
        public UserFeedSubscribeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.user_id, t.subscribe_type, t.target_id });

            // Properties
            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.target_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_userfeed_subscribe", "social");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.subscribe_type).HasColumnName("subscribe_type");
            this.Property(t => t.target_id).HasColumnName("target_id");

            // Relationships
            this.HasRequired(t => t.user).WithMany().HasForeignKey(d => d.user_id);
        }
    }
}