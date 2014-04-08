namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// Map for OAuth token.
    /// </summary>
    internal sealed class OAuthTokenMap : EntityTypeConfiguration<OAuthToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthTokenMap"/> class.
        /// </summary>
        public OAuthTokenMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_oauth_tokens");

            this.Property(t => t.ProviderId).HasColumnName("provider_id");
            this.Property(t => t.Id).HasColumnName("token_id");
            this.Property(t => t.ProviderUserId).HasColumnName("provider_user_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}