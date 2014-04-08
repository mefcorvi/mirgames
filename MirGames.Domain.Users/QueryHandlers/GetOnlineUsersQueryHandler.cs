namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class GetOnlineUsersQueryHandler : QueryHandler<GetOnlineUsersQuery, OnlineUserViewModel>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOnlineUsersQueryHandler" /> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public GetOnlineUsersQueryHandler(IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(onlineUsersManager != null);

            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetOnlineUsersQuery query, ClaimsPrincipal principal)
        {
            return this.GetOnlineUsers().Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<OnlineUserViewModel> Execute(IReadContext readContext, GetOnlineUsersQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.GetOnlineUsers().OrderBy(u => u.LastRequestDate);
        }

        /// <summary>
        /// Gets the online users.
        /// </summary>
        /// <returns>Sequence of online users.</returns>
        private IEnumerable<OnlineUserViewModel> GetOnlineUsers()
        {
            return this.onlineUsersManager.GetOnlineUsers();
        }
    }
}