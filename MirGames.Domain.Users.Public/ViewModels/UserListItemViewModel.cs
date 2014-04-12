// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserListItemViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.ViewModels
{
    using System;

    /// <summary>
    /// The user.
    /// </summary>
    public sealed class UserListItemViewModel
    {
        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the last visit.
        /// </summary>
        public DateTime LastVisit { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public int UserRating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is online.
        /// </summary>
        public bool IsOnline { get; set; }
    }
}