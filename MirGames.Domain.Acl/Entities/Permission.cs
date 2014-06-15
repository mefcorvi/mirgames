// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Permission.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The permission entity.
    /// </summary>
    internal sealed class Permission
    {
        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the entity type identifier.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        public int? ActionId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is denied.
        /// </summary>
        public bool IsDenied { get; set; }

        /// <summary>
        /// Gets or sets the expired date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? ExpirationDate { get; set; }
    }
}
