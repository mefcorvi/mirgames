namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The get target map.
    /// </summary>
    public class GetTargetMap : EntityTypeConfiguration<geo_target>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTargetMap"/> class.
        /// </summary>
        public GetTargetMap()
        {
            // Primary Key
            this.HasKey(t => new { t.geo_type, t.geo_id, t.target_type, t.target_id });

            // Properties
            this.Property(t => t.geo_type).IsRequired().HasMaxLength(20);

            this.Property(t => t.geo_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.target_type).IsRequired().HasMaxLength(20);

            this.Property(t => t.target_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_geo_target", "social");
            this.Property(t => t.geo_type).HasColumnName("geo_type");
            this.Property(t => t.geo_id).HasColumnName("geo_id");
            this.Property(t => t.target_type).HasColumnName("target_type");
            this.Property(t => t.target_id).HasColumnName("target_id");
            this.Property(t => t.country_id).HasColumnName("country_id");
            this.Property(t => t.region_id).HasColumnName("region_id");
            this.Property(t => t.city_id).HasColumnName("city_id");

            // Relationships
            this.HasOptional(t => t.geo_city).WithMany(t => t.geo_target).HasForeignKey(d => d.city_id);
            this.HasOptional(t => t.geo_country).WithMany(t => t.geo_target).HasForeignKey(d => d.country_id);
            this.HasOptional(t => t.geo_region).WithMany(t => t.geo_target).HasForeignKey(d => d.region_id);
        }

        #endregion
    }
}