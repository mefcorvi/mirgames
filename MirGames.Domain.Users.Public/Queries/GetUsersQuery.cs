// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUsersQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Queries
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of users.
    /// </summary>
    public sealed class GetUsersQuery : Query<UserListItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersQuery"/> class.
        /// </summary>
        public GetUsersQuery()
        {
            this.Filter = UserTypes.Activated;
        }

        /// <summary>
        /// Type of the sorting.
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// The login.
            /// </summary>
            Login,

            /// <summary>
            /// The rating.
            /// </summary>
            Rating,

            /// <summary>
            /// The last visit.
            /// </summary>
            LastVisit
        }

        /// <summary>
        /// The user types.
        /// </summary>
        [Flags]
        public enum UserTypes
        {
            /// <summary>
            /// The activated users.
            /// </summary>
            Activated = 1,

            /// <summary>
            /// The not activated users.
            /// </summary>
            NotActivated = 2,

            /// <summary>
            /// The online users.
            /// </summary>
            Online = 4,

            /// <summary>
            /// The offline users.
            /// </summary>
            Offline = 8
        }

        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        public SortType SortBy { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        public UserTypes Filter { get; set; }

        /// <summary>
        /// Gets or sets the user identifiers.
        /// </summary>
        public IEnumerable<int> UserIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString { get; set; }
    }
}