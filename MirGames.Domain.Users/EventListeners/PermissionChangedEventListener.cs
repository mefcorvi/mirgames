// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PermissionChangedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.EventListeners
{
    using MirGames.Domain.Acl.Public.Events;
    using MirGames.Domain.Users.Security;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Handles the permission changing.
    /// </summary>
    internal sealed class PermissionChangedEventListener : EventListenerBase<PermissionChangedEvent>
    {
        /// <summary>
        /// The principal cache.
        /// </summary>
        private readonly IPrincipalCache principalCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionChangedEventListener" /> class.
        /// </summary>
        /// <param name="principalCache">The principal cache.</param>
        public PermissionChangedEventListener(IPrincipalCache principalCache)
        {
            this.principalCache = principalCache;
        }

        /// <inheritdoc />
        public override void Process(PermissionChangedEvent @event)
        {
            this.principalCache.Clear();
        }
    }
}