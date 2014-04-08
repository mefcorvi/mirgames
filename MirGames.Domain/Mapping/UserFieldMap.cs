namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The user field map.
    /// </summary>
    public class UserFieldMap : EntityTypeConfiguration<user_field>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldMap"/> class.
        /// </summary>
        public UserFieldMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.type).IsRequired().HasMaxLength(50);

            this.Property(t => t.name).IsRequired().HasMaxLength(255);

            this.Property(t => t.title).IsRequired().HasMaxLength(255);

            this.Property(t => t.pattern).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("prefix_user_field", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.pattern).HasColumnName("pattern");
        }
    }
}