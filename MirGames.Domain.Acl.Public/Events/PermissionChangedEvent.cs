// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PermissionChangedEvent.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Public.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when permission has changed.
    /// </summary>
    public sealed class PermissionChangedEvent : Event
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        public string[] Actions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is denied.
        /// </summary>
        public bool IsDenied { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Acl.PermissionChanged"; }
        }
    }
}
