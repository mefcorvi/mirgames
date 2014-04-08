namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The favorite tag map.
    /// </summary>
    public class FavoriteTagMap : EntityTypeConfiguration<FavoriteTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteTagMap"/> class.
        /// </summary>
        public FavoriteTagMap()
        {
            // Primary Key
            // this.HasKey(t => new { user_id = t.UserId, target_id = t.TargetId, target_type = t.TargetType, is_user = t.IsUser, text = t.Text });
            this.HasKey(t => t.TargetId);

            // Properties
            this.Property(t => t.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TargetId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TargetType).IsRequired().HasMaxLength(65532);

            this.Property(t => t.Text).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("prefix_favourite_tag", "social");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.TargetId).HasColumnName("target_id");
            this.Property(t => t.TargetType).HasColumnName("target_type");
            this.Property(t => t.IsUser).HasColumnName("is_user");
            this.Property(t => t.Text).HasColumnName("text");

            // Relationships
            this.HasRequired(t => t.User).WithMany(t => t.FavouriteTag).HasForeignKey(d => d.UserId);
        }
    }
}