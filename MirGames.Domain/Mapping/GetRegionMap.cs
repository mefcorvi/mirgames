namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The get region map.
    /// </summary>
    public class GetRegionMap : EntityTypeConfiguration<geo_region>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRegionMap"/> class.
        /// </summary>
        public GetRegionMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name_ru).IsRequired().HasMaxLength(50);

            this.Property(t => t.name_en).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("prefix_geo_region", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.country_id).HasColumnName("country_id");
            this.Property(t => t.name_ru).HasColumnName("name_ru");
            this.Property(t => t.name_en).HasColumnName("name_en");
            this.Property(t => t.sort).HasColumnName("sort");

            // Relationships
            this.HasRequired(t => t.geo_country).WithMany(t => t.geo_region).HasForeignKey(d => d.country_id);
        }

        #endregion
    }
}