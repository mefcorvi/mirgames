// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUserByIdQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetUserByIdQuery.
    /// </summary>
    internal sealed class GetUserByIdQueryHandler : SingleItemQueryHandler<GetUserByIdQuery, UserViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The avatar provider.
        /// </summary>
        private readonly IAvatarProvider avatarProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="avatarProvider">The avatar provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetUserByIdQueryHandler(
            IAuthorizationManager authorizationManager,
            IAvatarProvider avatarProvider,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(avatarProvider != null);
            Contract.Requires(readContextFactory != null);

            this.authorizationManager = authorizationManager;
            this.avatarProvider = avatarProvider;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override UserViewModel Execute(GetUserByIdQuery query, ClaimsPrincipal principal)
        {
            User user;
            using (var readContext = this.readContextFactory.Create())
            {
                user = readContext
                    .Query<User>()
                    .SingleOrDefault(u => u.Id == query.UserId);
            }

            if (user == null)
            {
                return null;
            }

            return new UserViewModel
                {
                    AvatarUrl = user.AvatarUrl ?? this.avatarProvider.GetAvatarUrl(user.Mail, user.Login),
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name,
                    About = user.About,
                    Birthday = user.Birthday,
                    Location = user.Location,
                    RegistrationDate = user.RegistrationDate,
                    LastVisitDate = user.LastVisitDate,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "User", user.Id),
                    CanReceiveMessage = this.authorizationManager.CheckAccess(principal, "SendMessage", "User", user.Id),
                    WallRecordCanBeAdded = this.authorizationManager.CheckAccess(principal, "PostWallRecord", "User", user.Id),
                    Rating = user.UserRating
                };
        }
    }
}