// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserSession.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Entities
{
    using System;

    /// <summary>
    /// The user session.
    /// </summary>
    internal class UserSession
    {
        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the creation IP.
        /// </summary>
        public string CreationIP { get; set; }

        /// <summary>
        /// Gets or sets the last visit IP.
        /// </summary>
        public string LastVisitIP { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the last date.
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}
