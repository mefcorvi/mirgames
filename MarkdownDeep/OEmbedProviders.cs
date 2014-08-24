namespace Xilium.MarkdownDeep
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OEmbed.Net;
    using OEmbed.Net.Domain;
    using OEmbed.Net.Utilities;

    /// <summary>
    /// The embed providers.
    /// </summary>
    internal static class OEmbedProviders
    {
        /// <summary>
        /// The providers.
        /// </summary>
        private static readonly IEnumerable<OEmbedProviderBase> Providers;

        /// <summary>
        /// Initializes static members of the <see cref="OEmbedProviders"/> class.
        /// </summary>
        static OEmbedProviders()
        {
            Providers = new List<OEmbedProviderBase>
                {
                    new OEmbedProvider<Video>("http://www.youtube.com/oembed", "youtube\\.com/watch.+v=[\\w-]+&?", "YouTube"),
                    new OEmbedProvider<Video>("http://www.youtube.com/oembed", "youtu\\.be/[\\w-]+[\\?]?", "YouTu.be"),
                    new OEmbedProvider<Video>("http://www.hulu.com/api/oembed.json", "hulu\\.com/watch/.*", "Hulu"),
                    new OEmbedProvider<Video>("http://www.vimeo.com/api/oembed.json", "vimeo\\.com/.*", "Vimeo"),
                    new OEmbedProvider<Photo>("http://www.flickr.com/services/oembed/", "flickr\\.com/photos/[-.\\w@]+/\\d+/?", "Flickr Photos")
                };
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="url">The URL.</param>
        /// <returns>The object.</returns>
        public static T GetObject<T>(string url) where T : Base
        {
            var provider = Providers.OfType<OEmbedProvider<T>>().FirstOrDefault(item => item.UrlRegex.IsMatch(url));

            return provider == null ? null : provider.GetObject(url);
        }
    }

    /// <summary>
    /// Base class of embed provider.
    /// </summary>
    internal abstract class OEmbedProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OEmbedProviderBase"/> class.
        /// </summary>
        /// <param name="endpointUrl">The endpoint URL.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="title">The title.</param>
        protected OEmbedProviderBase(string endpointUrl, string regex, string title)
        {
            this.EndpointUrl = endpointUrl;
            this.UrlRegex = new Regex(regex);
            this.Title = title;
        }

        /// <summary>
        /// Gets the endpoint URL.
        /// </summary>
        public string EndpointUrl { get; private set; }

        /// <summary>
        /// Gets the URL regex.
        /// </summary>
        public Regex UrlRegex { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; private set; }
    }

    /// <summary>
    /// The Embed Data Provider.
    /// </summary>
    internal class OEmbedProvider<T> : OEmbedProviderBase where T : Base
    {
        /// <summary>
        /// The consumer.
        /// </summary>
        private readonly Consumer<T> consumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OEmbedProvider{T}"/> class.
        /// </summary>
        /// <param name="endpointUrl">The endpoint URL.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="title">The title.</param>
        public OEmbedProvider(string endpointUrl, string regex, string title)
            : base(endpointUrl, regex, title)
        {
            this.consumer = new Consumer<T>();
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The object.</returns>
        public T GetObject(string url)
        {
            return this.consumer.GetObject(new ServiceCallBuilder(this.EndpointUrl, url));
        }
    }
}
