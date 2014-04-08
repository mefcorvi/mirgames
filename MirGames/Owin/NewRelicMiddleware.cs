namespace MirGames.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    /// <summary>
    /// The new relic middleware.
    /// </summary>
    public class NewRelicMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewRelicMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware.</param>
        public NewRelicMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <inheritdoc />
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("signalr"))
            {
                NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
            }

            await this.Next.Invoke(context);
        }
    }
}