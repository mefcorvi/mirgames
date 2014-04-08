namespace MirGames.OAuth
{
    using System.Diagnostics.Contracts;

    using DotNetOpenAuth.AspNet;
    using DotNetOpenAuth.AspNet.Clients;

    using MirGames.Infrastructure;

    /// <summary>
    /// The facebook client.
    /// </summary>
    internal sealed class FacebookAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationClientProvider"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public FacebookAuthenticationClientProvider(ISettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new FacebookClient(this.settings.GetValue<string>("Facebook.AppID"), this.settings.GetValue<string>("Facebook.AppSecret"));
        }
    }
}