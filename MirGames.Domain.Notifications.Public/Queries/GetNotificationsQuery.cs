// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNotificationsQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Queries
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Gets the notifications.
    /// </summary>
    public sealed class GetNotificationsQuery : Query<NotificationViewModel>
    {
        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the is read.
        /// </summary>
        public bool? IsRead { get; set; }

        /// <summary>
        /// Gets or sets the entity identifiers.
        /// </summary>
        public Expression<Func<NotificationData, bool>> Filter { get; set; }

        /// <summary>
        /// Sets the filter.
        /// </summary>
        /// <typeparam name="T">Type of the data.</typeparam>
        /// <returns>The query.</returns>
        public GetNotificationsQuery WithFilter<T>() where T : NotificationData, new()
        {
            var instance = new T();
            this.NotificationType = instance.NotificationType;

            return this;
        }

        /// <summary>
        /// Sets the filter.
        /// </summary>
        /// <typeparam name="T">Type of the data.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns>The query.</returns>
        public GetNotificationsQuery WithFilter<T>(Expression<Func<T, bool>> filter) where T : NotificationData, new()
        {
            var instance = new T();
            this.NotificationType = instance.NotificationType;

            Expression<Func<NotificationData, T>> substitution = d => (T)d;
            var visitor = new ParameterReplacerVisitor<NotificationData, T, bool>(filter.Parameters.First(), substitution);
            var convertedExpression = visitor.VisitAndConvert(filter);

            this.Filter = convertedExpression;

            return this;
        }
    }
}
