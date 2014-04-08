namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The geo city map.
    /// </summary>
    public class GeoCityMap : EntityTypeConfiguration<geo_city>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCityMap"/> class.
        /// </summary>
        public GeoCityMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name_ru).IsRequired().HasMaxLength(50);

            this.Property(t => t.name_en).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("prefix_geo_city", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.country_id).HasColumnName("country_id");
            this.Property(t => t.region_id).HasColumnName("region_id");
            this.Property(t => t.name_ru).HasColumnName("name_ru");
            this.Property(t => t.name_en).HasColumnName("name_en");
            this.Property(t => t.sort).HasColumnName("sort");

            // Relationships
            this.HasRequired(t => t.geo_country).WithMany(t => t.geo_city).HasForeignKey(d => d.country_id);
            this.HasRequired(t => t.geo_region).WithMany(t => t.geo_city).HasForeignKey(d => d.region_id);
        }

        #endregion
    }
}