namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The config item map.
    /// </summary>
    internal class ConfigItemMap : EntityTypeConfiguration<ConfigItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigItemMap"/> class.
        /// </summary>
        public ConfigItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Key);

            // Properties
            this.Property(t => t.Value).HasMaxLength(2048);

            // Table & Column Mappings
            this.ToTable("config");
        }
    }
}