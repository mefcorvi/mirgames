// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUsersQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
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
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class GetUsersQueryHandler : QueryHandler<GetUsersQuery, UserListItemViewModel>
    {
        /// <summary>
        /// The avatar provider.
        /// </summary>
        private readonly IAvatarProvider avatarProvider;

        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersQueryHandler" /> class.
        /// </summary>
        /// <param name="avatarProvider">The avatar provider.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public GetUsersQueryHandler(IAvatarProvider avatarProvider, IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(avatarProvider != null);
            Contract.Requires(onlineUsersManager != null);

            this.avatarProvider = avatarProvider;
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetUsersQuery query, ClaimsPrincipal principal)
        {
            return this.GetUsersQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<UserListItemViewModel> Execute(IReadContext readContext, GetUsersQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var users = this.GetUsersQuery(readContext, query).Select(
                u => new
                {
                    u.AvatarUrl,
                    u.Id,
                    u.Login,
                    u.Location,
                    u.Name,
                    u.RegistrationDate,
                    u.UserRating,
                    u.LastVisitDate,
                    u.Mail
                });

            var onlineUserIdentifiers = new HashSet<int>(this.onlineUsersManager.GetOnlineUsers().Select(user => user.Id.GetValueOrDefault()));

            return this.ApplyPagination(users, pagination).ToList().Select(u => new UserListItemViewModel
                {
                    AvatarUrl = u.AvatarUrl ?? this.avatarProvider.GetAvatarUrl(u.Mail, u.Login),
                    Id = u.Id,
                    Login = u.Login,
                    Location = u.Location,
                    Name = u.Name,
                    RegistrationDate = u.RegistrationDate,
                    UserRating = u.UserRating,
                    LastVisit = u.LastVisitDate,
                    IsOnline = onlineUserIdentifiers.Contains(u.Id)
                });
        }

        /// <summary>
        /// Gets the users query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The users query.</returns>
        private IQueryable<User> GetUsersQuery(IReadContext readContext, GetUsersQuery query)
        {
            IQueryable<User> set = readContext.Query<User>();

            if (query.Filter.HasFlag(Queries.GetUsersQuery.UserTypes.Activated))
            {
                set = set.Where(u => u.IsActivated);
            }

            if (query.Filter.HasFlag(Queries.GetUsersQuery.UserTypes.NotActivated))
            {
                set = set.Where(u => !u.IsActivated);
            }

            if (query.Filter.HasFlag(Queries.GetUsersQuery.UserTypes.Online))
            {
                var onlineUsers = this.onlineUsersManager.GetOnlineUsers().Select(user => user.Id.GetValueOrDefault()).EnsureCollection();
                set = set.Where(x => onlineUsers.Contains(x.Id));
            }

            if (query.Filter.HasFlag(Queries.GetUsersQuery.UserTypes.Offline))
            {
                var onlineDate = DateTime.UtcNow.AddMinutes(-5);
                set = set.Where(x => x.LastVisitDate < onlineDate);
            }

            if (query.UserIdentifiers != null)
            {
                int[] identifiers = query.UserIdentifiers.ToArray();
                set = set.Where(x => identifiers.Contains(x.Id));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchString))
            {
                set = set.Where(x => x.Name.Contains(query.SearchString));
            }

            switch (query.SortBy)
            {
                case Queries.GetUsersQuery.SortType.Login:
                    set = set.OrderBy(x => x.Login);
                    break;
                case Queries.GetUsersQuery.SortType.Rating:
                    set = set.OrderByDescending(x => x.UserRating);
                    break;
                case Queries.GetUsersQuery.SortType.LastVisit:
                    set = set.OrderByDescending(x => x.LastVisitDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return set;
        }
    }
}