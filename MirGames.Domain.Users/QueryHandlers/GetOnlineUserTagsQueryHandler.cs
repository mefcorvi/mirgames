namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns map between user identifier and tags.
    /// </summary>
    internal sealed class GetOnlineUserTagsQueryHandler : SingleItemQueryHandler<GetOnlineUserTagsQuery, IDictionary<int, IEnumerable<string>>>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOnlineUserTagsQueryHandler"/> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public GetOnlineUserTagsQueryHandler(IOnlineUsersManager onlineUsersManager)
        {
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        public override IDictionary<int, IEnumerable<string>> Execute(IReadContext readContext, GetOnlineUserTagsQuery query, ClaimsPrincipal principal)
        {
            return this.onlineUsersManager.GetUserTags();
        }
    }
}