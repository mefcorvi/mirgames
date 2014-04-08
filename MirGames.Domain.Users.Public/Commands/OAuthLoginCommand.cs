namespace MirGames.Domain.Users.Commands
{
    using System.Diagnostics.CodeAnalysis;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Logins with OAuth credentials.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [DisableTracing]
    public class OAuthLoginCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the provider user unique identifier.
        /// </summary>
        public string ProviderUserId { get; set; }
    }
}