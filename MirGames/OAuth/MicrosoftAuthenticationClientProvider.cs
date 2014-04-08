namespace MirGames.OAuth
{
    using DotNetOpenAuth.AspNet;
    using DotNetOpenAuth.AspNet.Clients;

    using MirGames.Infrastructure;

    /// <summary>
    /// The microsoft authentication provider.
    /// </summary>
    internal sealed class MicrosoftAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftAuthenticationClientProvider"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MicrosoftAuthenticationClientProvider(ISettings settings)
        {
            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new MicrosoftClient(
                this.settings.GetValue<string>("Microsoft.AppID"), this.settings.GetValue<string>("Microsoft.AppSecret"));
        }
    }
}