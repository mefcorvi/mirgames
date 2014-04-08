namespace MirGames.Domain.Users.ViewModels
{
    /// <summary>
    /// The OAuth provider view model.
    /// </summary>
    public sealed class OAuthProviderViewModel
    {
        /// <summary>
        /// Gets or sets the provider unique identifier.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user is linked with the provider.
        /// </summary>
        public bool IsLinked { get; set; }
    }
}