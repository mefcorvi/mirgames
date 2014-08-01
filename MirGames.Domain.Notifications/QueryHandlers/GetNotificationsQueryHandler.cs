// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetNotificationsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Entities;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.Services;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    using MongoDB.Driver.Linq;

    /// <summary>
    /// Gets the notifications.
    /// </summary>
    internal sealed class GetNotificationsQueryHandler : QueryHandler<GetNotificationsQuery, NotificationViewModel>
    {
        /// <summary>
        /// The mongo database factory.
        /// </summary>
        private readonly IMongoDatabaseFactory mongoDatabaseFactory;

        /// <summary>
        /// The event type resolver.
        /// </summary>
        private readonly INotificationTypeResolver notificationTypeResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationsQueryHandler" /> class.
        /// </summary>
        /// <param name="mongoDatabaseFactory">The mongo database factory.</param>
        /// <param name="notificationTypeResolver">The event type resolver.</param>
        public GetNotificationsQueryHandler(IMongoDatabaseFactory mongoDatabaseFactory, INotificationTypeResolver notificationTypeResolver)
        {
            Contract.Requires(notificationTypeResolver != null);
            Contract.Requires(mongoDatabaseFactory != null);

            this.mongoDatabaseFactory = mongoDatabaseFactory;
            this.notificationTypeResolver = notificationTypeResolver;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetNotificationsQuery query, ClaimsPrincipal principal)
        {
            return this.GetQuery(query, principal).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationViewModel> Execute(
            IReadContext readContext,
            GetNotificationsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            return
                this.ApplyPagination(this.GetQuery(query, principal), pagination)
                    .ToList()
                    .Select(n => new NotificationViewModel
                    {
                        NotificationType = query.NotificationType,
                        Data = n.Data,
                        NotificationId = n.Id.ToString(),
                        UserId = n.UserId
                    });
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The queryable.</returns>
        private IQueryable<Notification> GetQuery(GetNotificationsQuery query, ClaimsPrincipal principal)
        {
            var userId = principal.GetUserId().GetValueOrDefault();

            var notificationsQuery =
                this.mongoDatabaseFactory.CreateDatabase().GetCollection<Notification>("notifications").AsQueryable();

            notificationsQuery = notificationsQuery.Where(n => n.UserId == userId);

            if (query.NotificationType != null)
            {
                var notificationTypeId = this.notificationTypeResolver.GetIdentifier(query.NotificationType);
                notificationsQuery = notificationsQuery.Where(n => n.NotificationTypeId == notificationTypeId);
            }

            if (query.Filter != null)
            {
                Expression<Func<Notification, NotificationData>> targetExpression = n => n.Data;
                var visitor = new ParameterReplacerVisitor<Notification, NotificationData, bool>(query.Filter.Parameters.First(), targetExpression);

                var convertedExpression = visitor.VisitAndConvert(query.Filter);
                notificationsQuery = notificationsQuery.Where(convertedExpression);
            }

            return notificationsQuery;
        }
    }
}
