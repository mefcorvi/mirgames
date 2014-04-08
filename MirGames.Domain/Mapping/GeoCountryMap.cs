namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The geo country map.
    /// </summary>
    public class GeoCountryMap : EntityTypeConfiguration<geo_country>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCountryMap"/> class.
        /// </summary>
        public GeoCountryMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name_ru).IsRequired().HasMaxLength(50);

            this.Property(t => t.name_en).IsRequired().HasMaxLength(50);

            this.Property(t => t.code).IsRequired().HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("prefix_geo_country", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name_ru).HasColumnName("name_ru");
            this.Property(t => t.name_en).HasColumnName("name_en");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.sort).HasColumnName("sort");
        }

        #endregion
    }
}