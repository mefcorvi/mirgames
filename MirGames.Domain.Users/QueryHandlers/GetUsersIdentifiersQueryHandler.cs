// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUsersIdentifiersQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class GetUsersIdentifiersQueryHandler : QueryHandler<GetUsersIdentifiersQuery, int>
    {
        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersIdentifiersQueryHandler" /> class.
        /// </summary>
        /// <param name="onlineUsersManager">The online users manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetUsersIdentifiersQueryHandler(IOnlineUsersManager onlineUsersManager, IReadContextFactory readContextFactory)
        {
            Contract.Requires(onlineUsersManager != null);
            Contract.Requires(readContextFactory != null);

            this.onlineUsersManager = onlineUsersManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetUsersIdentifiersQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetUsersQuery(readContext, query).Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<int> Execute(GetUsersIdentifiersQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var users = this.GetUsersQuery(readContext, query).Select(u => u.Id);
                return this.ApplyPagination(users, pagination).ToList();
            }
        }

        /// <summary>
        /// Gets the users query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The users query.</returns>
        private IQueryable<User> GetUsersQuery(IReadContext readContext, GetUsersIdentifiersQuery query)
        {
            IQueryable<User> set = readContext.Query<User>();

            if (query.Filter.HasFlag(GetUsersIdentifiersQuery.UserTypes.Activated))
            {
                set = set.Where(u => u.IsActivated);
            }

            if (query.Filter.HasFlag(GetUsersIdentifiersQuery.UserTypes.NotActivated))
            {
                set = set.Where(u => !u.IsActivated);
            }

            if (query.Filter.HasFlag(GetUsersIdentifiersQuery.UserTypes.Online))
            {
                var onlineUsers = this.onlineUsersManager.GetOnlineUsers().Select(user => user.Id.GetValueOrDefault()).EnsureCollection();
                set = set.Where(x => onlineUsers.Contains(x.Id));
            }

            if (query.Filter.HasFlag(GetUsersIdentifiersQuery.UserTypes.Offline))
            {
                var onlineDate = DateTime.UtcNow.AddMinutes(-5);
                set = set.Where(x => x.LastVisitDate < onlineDate);
            }

            return set;
        }
    }
}