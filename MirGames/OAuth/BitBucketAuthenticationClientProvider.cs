namespace MirGames.OAuth
{
    using System.Diagnostics.Contracts;

    using DevAuth.AspNet;

    using DotNetOpenAuth.AspNet;

    using MirGames.Infrastructure;

    /// <summary>
    /// The GitHub authentication client provider.
    /// </summary>
    internal sealed class BitBucketAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitBucketAuthenticationClientProvider"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public BitBucketAuthenticationClientProvider(ISettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new BitBucketAuthenticationClient(this.settings.GetValue<string>("BitBucket.AppID"), this.settings.GetValue<string>("BitBucket.AppSecret"));
        }
    }
}