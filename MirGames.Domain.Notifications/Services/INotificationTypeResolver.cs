// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="INotificationTypeResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Services
{
    /// <summary>
    /// Resolves the event type.
    /// </summary>
    internal interface INotificationTypeResolver
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>An identifier of the event.</returns>
        int GetIdentifier(string eventType);

        /// <summary>
        /// Gets the type of the notification.
        /// </summary>
        /// <param name="eventTypeId">The event type identifier.</param>
        /// <returns>The type of the notification class.</returns>
        string GetNotificationType(int eventTypeId);
    }
}