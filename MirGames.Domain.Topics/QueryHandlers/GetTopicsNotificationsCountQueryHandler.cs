namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetTopicsNotificationsCountQueryHandler : SingleItemQueryHandler<GetTopicsNotificationsCountQuery, int>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsNotificationsCountQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetTopicsNotificationsCountQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int Execute(IReadContext readContext, GetTopicsNotificationsCountQuery query, ClaimsPrincipal principal)
        {
            var notificationsQuery = new GetNotificationsQuery
            {
                Filter = n => n is NewBlogTopicNotification || n is NewTopicCommentNotification
            };

            return this.queryProcessor.GetItemsCount(notificationsQuery);
        }
    }
}
