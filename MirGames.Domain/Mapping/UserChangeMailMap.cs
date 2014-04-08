namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The user change mail map.
    /// </summary>
    public class UserChangeMailMap : EntityTypeConfiguration<user_changemail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserChangeMailMap"/> class.
        /// </summary>
        public UserChangeMailMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.mail_from).IsRequired().HasMaxLength(50);

            this.Property(t => t.mail_to).IsRequired().HasMaxLength(50);

            this.Property(t => t.code_from).IsRequired().HasMaxLength(32);

            this.Property(t => t.code_to).IsRequired().HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("prefix_user_changemail", "social");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.user_id).HasColumnName("user_id");
            this.Property(t => t.date_add).HasColumnName("date_add");
            this.Property(t => t.date_used).HasColumnName("date_used");
            this.Property(t => t.date_expired).HasColumnName("date_expired");
            this.Property(t => t.mail_from).HasColumnName("mail_from");
            this.Property(t => t.mail_to).HasColumnName("mail_to");
            this.Property(t => t.code_from).HasColumnName("code_from");
            this.Property(t => t.code_to).HasColumnName("code_to");
            this.Property(t => t.confirm_from).HasColumnName("confirm_from");
            this.Property(t => t.confirm_to).HasColumnName("confirm_to");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.user_changemail).HasForeignKey(d => d.user_id);
        }
    }
}