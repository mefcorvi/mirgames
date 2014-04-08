namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The user field value map.
    /// </summary>
    public class UserFieldValueMap : EntityTypeConfiguration<user_field_value>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFieldValueMap"/> class.
        /// </summary>
        public UserFieldValueMap()
        {
            // Primary Key
            this.HasKey(t => t.user_id);

            // Properties
            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.value).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("prefix_user_field_value", "social");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.field_id).HasColumnName("field_id");
            this.Property(t => t.value).HasColumnName("value");

            // Relationships
            this.HasRequired(t => t.user).WithOptional(t => t.user_field_value);
            this.HasOptional(t => t.user_field).WithMany(t => t.user_field_value).HasForeignKey(d => d.field_id);
        }
    }
}