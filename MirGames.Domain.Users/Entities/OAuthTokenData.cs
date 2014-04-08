namespace MirGames.Domain.Users.Entities
{
    /// <summary>
    /// The OAuth token data.
    /// </summary>
    internal sealed class OAuthTokenData
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the token unique identifier.
        /// </summary>
        public int TokenId { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
    }
}