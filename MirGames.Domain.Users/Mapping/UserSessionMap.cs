namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The session map.
    /// </summary>
    internal class UserSessionMap : EntityTypeConfiguration<UserSession>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionMap"/> class.
        /// </summary>
        public UserSessionMap()
        {
            this.ToTable("prefix_session");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("session_key").IsRequired().HasMaxLength(32);
            this.Property(t => t.CreationIP).HasColumnName("session_ip_create").IsRequired();
            this.Property(t => t.LastVisitIP).HasColumnName("session_ip_last").IsRequired();
            this.Property(t => t.CreateDate).HasColumnName("session_date_create").IsRequired();
            this.Property(t => t.LastDate).HasColumnName("session_date_last").IsRequired();
            this.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
        }
    }
}