namespace MirGames.Domain.Users.Entities
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The OAuth token.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed class OAuthToken
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the provider unique identifier.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the provider user unique identifier.
        /// </summary>
        public string ProviderUserId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}