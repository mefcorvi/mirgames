// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetUsersIdentifiersQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Queries
{
    using System;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of users.
    /// </summary>
    public sealed class GetUsersIdentifiersQuery : Query<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersIdentifiersQuery"/> class.
        /// </summary>
        public GetUsersIdentifiersQuery()
        {
            this.Filter = UserTypes.Activated;
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
        /// Gets or sets the filter.
        /// </summary>
        public UserTypes Filter { get; set; }
    }
}