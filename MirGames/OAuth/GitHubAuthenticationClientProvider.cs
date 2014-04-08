namespace MirGames.OAuth
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using DevAuth.AspNet;

    using DotNetOpenAuth.AspNet;

    using MirGames.Infrastructure;

    /// <summary>
    /// The GitHub authentication client provider.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed class GitHubAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAuthenticationClientProvider"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public GitHubAuthenticationClientProvider(ISettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new GitHubAuthenticationClient(this.settings.GetValue<string>("GitHub.AppID"), this.settings.GetValue<string>("GitHub.AppSecret"));
        }
    }
}