// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RemoveNotificationsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Notifications.Entities;
    using MirGames.Domain.Notifications.Events;
    using MirGames.Domain.Notifications.Services;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    using MongoDB.Bson;
    using MongoDB.Driver.Linq;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class RemoveNotificationsCommandHandler : CommandHandler<RemoveNotificationsCommand, int>
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
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveNotificationsCommandHandler" /> class.
        /// </summary>
        /// <param name="mongoDatabaseFactory">The mongo database factory.</param>
        /// <param name="notificationTypeResolver">The event type resolver.</param>
        /// <param name="eventBus">The event bus.</param>
        public RemoveNotificationsCommandHandler(
            IMongoDatabaseFactory mongoDatabaseFactory,
            INotificationTypeResolver notificationTypeResolver,
            IEventBus eventBus)
        {
            Contract.Requires(mongoDatabaseFactory != null);
            Contract.Requires(notificationTypeResolver != null);
            Contract.Requires(eventBus != null);

            this.mongoDatabaseFactory = mongoDatabaseFactory;
            this.notificationTypeResolver = notificationTypeResolver;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override int Execute(
            RemoveNotificationsCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            var collection = this.mongoDatabaseFactory.CreateDatabase().GetCollection<Notification>("notifications");

            var notificationsQuery = collection.AsQueryable();

            if (!string.IsNullOrEmpty(command.NotificationType))
            {
                var notificationTypeId = this.notificationTypeResolver.GetIdentifier(command.NotificationType);
                notificationsQuery = notificationsQuery.Where(n => n.NotificationTypeId == notificationTypeId);
            }

            if (command.NotificationIdentifiers != null && command.NotificationIdentifiers.Length > 0)
            {
                var identifiers = command.NotificationIdentifiers.Select(ObjectId.Parse);
                notificationsQuery = notificationsQuery.Where(n => identifiers.Contains(n.Id));
            }

            if (command.UserIdentifiers != null)
            {
                notificationsQuery = notificationsQuery.Where(n => command.UserIdentifiers.Contains(n.UserId));
            }

            if (command.Filter != null)
            {
                Expression<Func<Notification, NotificationData>> targetExpression = n => n.Data;
                var visitor = new ParameterReplacerVisitor<Notification, NotificationData, bool>(command.Filter.Parameters.First(), targetExpression);

                var convertedExpression = visitor.VisitAndConvert(command.Filter);
                notificationsQuery = notificationsQuery.Where(convertedExpression);
            }

            var notifications = notificationsQuery
                .ToArray()
                .Select(n => new NotificationViewModel
                {
                    Data = n.Data,
                    NotificationId = n.Id.ToString(),
                    NotificationType = this.notificationTypeResolver.GetNotificationType(n.NotificationTypeId),
                    UserId = n.UserId
                })
                .ToArray();

            if (notifications.Length > 0)
            {
                var mongoQuery =
                    new MongoQueryProvider(collection).BuildMongoQuery((MongoQueryable<Notification>)notificationsQuery);
                collection.Remove(mongoQuery);

                this.eventBus.Raise(new NotificationsRemovedEvent
                {
                    Notifications = notifications
                });
            }

            return notifications.Length;
        }
    }
}