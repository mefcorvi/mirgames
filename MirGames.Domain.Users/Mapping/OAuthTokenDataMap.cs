namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// Map for OAuth token data.
    /// </summary>
    internal sealed class OAuthTokenDataMap : EntityTypeConfiguration<OAuthTokenData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthTokenDataMap"/> class.
        /// </summary>
        public OAuthTokenDataMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_oauth_data");

            this.Property(t => t.Id).HasColumnName("data_id");
            this.Property(t => t.TokenId).HasColumnName("token_id");
            this.Property(t => t.Key).HasColumnName("key");
            this.Property(t => t.Value).HasColumnName("value");
        }
    }
}