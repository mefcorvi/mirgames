namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetForumNotificationsCountQueryHandler : SingleItemQueryHandler<GetForumNotificationsCountQuery, int>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumNotificationsCountQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetForumNotificationsCountQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int Execute(IReadContext readContext, GetForumNotificationsCountQuery query, ClaimsPrincipal principal)
        {
            var notificationsQuery = new GetNotificationsQuery
            {
                Filter = n => n is NewForumAnswerNotification || n is NewForumTopicNotification
            };
            return this.queryProcessor.GetItemsCount(notificationsQuery);
        }
    }
}
