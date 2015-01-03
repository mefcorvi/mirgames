// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNotificationDetailsQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Queries
{
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the notification details.
    /// </summary>
    public sealed class GetNotificationDetailsQuery : Query<NotificationDetailsViewModel>
    {
        /// <summary>
        /// Gets or sets the notifications.
        /// </summary>
        public NotificationData[] Notifications { get; set; }
    }
}