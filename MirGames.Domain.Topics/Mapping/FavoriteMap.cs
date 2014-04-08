namespace MirGames.Domain.Topics.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The favorite map.
    /// </summary>
    internal class FavoriteMap : EntityTypeConfiguration<Favorite>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteMap"/> class.
        /// </summary>
        public FavoriteMap()
        {
            // Primary Key
            this.HasKey(t => t.TargetId);

            // Properties
            this.Property(t => t.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TargetType).HasMaxLength(65532);

            this.Property(t => t.Tags).IsRequired().HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("prefix_favourite");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.TargetId).HasColumnName("target_id");
            this.Property(t => t.TargetType).HasColumnName("target_type");
            this.Property(t => t.TargetPublish).HasColumnName("target_publish");
            this.Property(t => t.Tags).HasColumnName("tags");
        }

        #endregion
    }
}