namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The password restore request map.
    /// </summary>
    internal class PasswordRestoreRequestMap : EntityTypeConfiguration<PasswordRestoreRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordRestoreRequestMap"/> class.
        /// </summary>
        public PasswordRestoreRequestMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_password_restore");
            this.Property(t => t.CreationDate).HasColumnName("creation_date");
            this.Property(t => t.Id).HasColumnName("password_restore_id");
            this.Property(t => t.NewPassword).HasColumnName("new_password").IsRequired().HasMaxLength(255);
            this.Property(t => t.SecretKey).HasColumnName("secret_key").IsRequired().HasMaxLength(255);
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}