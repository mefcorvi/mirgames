namespace MirGames.OAuth
{
    using System.Diagnostics.Contracts;

    using DotNetOpenAuth.AspNet;
    using DotNetOpenAuth.GoogleOAuth2;

    using MirGames.Infrastructure;

    /// <summary>
    /// The google authentication client.
    /// </summary>
    internal sealed class GoogleAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAuthenticationClientProvider" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public GoogleAuthenticationClientProvider(ISettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new GoogleOAuth2Client(this.settings.GetValue<string>("Google.AppID"), this.settings.GetValue<string>("Google.AppSecret"));
        }
    }
}