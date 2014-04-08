namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The OAuth provider map.
    /// </summary>
    internal sealed class OAuthProviderMap : EntityTypeConfiguration<OAuthProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProviderMap"/> class.
        /// </summary>
        public OAuthProviderMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_oauth_providers");
            this.Property(t => t.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            this.Property(t => t.DisplayName).HasColumnName("display_name").IsRequired().HasMaxLength(255);
            this.Property(t => t.Id).HasColumnName("provider_id");
        }
    }
}