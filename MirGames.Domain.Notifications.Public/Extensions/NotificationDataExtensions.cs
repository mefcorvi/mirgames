// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotificationDataExtensions.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MirGames.Domain.Notifications.ViewModels;

    /// <summary>
    /// Extensions for the notification data.
    /// </summary>
    public static class NotificationDataExtensions
    {
        public static IEnumerable<T> CopyBaseNotificationData<T, TDetails, TKey>(
            this IEnumerable<T> details,
            IEnumerable<TDetails> notifications,
            Func<T, TKey> detailsKeySelector,
            Func<TDetails, TKey> notificationsKeySelector)
            where T : NotificationDetailsViewModel
            where TDetails : NotificationData
        {
            return details.Join(
                notifications,
                detailsKeySelector,
                notificationsKeySelector,
                (detail, notification) =>
                {
                    detail.NotificationType = notification.NotificationType;
                    detail.NotificationDate = notification.NotificationDate;
                    detail.IsRead = notification.IsRead;
                    return detail;
                });
        }
    }
}
