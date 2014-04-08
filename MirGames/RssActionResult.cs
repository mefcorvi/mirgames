namespace MirGames
{
    using System;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;
    using System.Xml;

    /// <summary>
    /// The RSS action result.
    /// </summary>
    public class RssActionResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssActionResult"/> class.
        /// </summary>
        /// <param name="feed">The feed.</param>
        public RssActionResult(SyndicationFeed feed) : this(feed, FeedFormat.Rss)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssActionResult" /> class.
        /// </summary>
        /// <param name="feed">The feed.</param>
        /// <param name="format">The format of syndication (RSS or atom).</param>
        public RssActionResult(SyndicationFeed feed, FeedFormat format)
        {
            this.Feed = feed;
            this.Format = format;
        }

        /// <summary>
        /// The feed format.
        /// </summary>
        public enum FeedFormat
        {
            /// <summary>
            /// The RSS.
            /// </summary>
            Rss,

            /// <summary>
            /// The atom.
            /// </summary>
            Atom
        }

        /// <summary>
        /// Gets or sets the feed.
        /// </summary>
        public SyndicationFeed Feed { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public FeedFormat Format { get; set; }

        /// <inheritdoc />
        public override void ExecuteResult(ControllerContext context)
        {
            SyndicationFeedFormatter rssFormatter;

            switch (this.Format)
            {
                case FeedFormat.Rss:
                    context.HttpContext.Response.ContentType = "text/xml";
                    rssFormatter = new Rss20FeedFormatter(this.Feed);
                    break;
                case FeedFormat.Atom:
                    context.HttpContext.Response.ContentType = "application/atom+xml";
                    rssFormatter = new Atom10FeedFormatter(this.Feed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}