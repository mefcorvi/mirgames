// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotifyUserCommandHandler.cs">
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
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Notifications.Entities;
    using MirGames.Domain.Notifications.Events;
    using MirGames.Domain.Notifications.Services;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class NotifyUserCommandHandler : CommandHandler<NotifyUsersCommand>
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
        /// Initializes a new instance of the <see cref="NotifyUserCommandHandler" /> class.
        /// </summary>
        /// <param name="mongoDatabaseFactory">The mongo database factory.</param>
        /// <param name="notificationTypeResolver">The event type resolver.</param>
        /// <param name="eventBus">The event bus.</param>
        public NotifyUserCommandHandler(
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
        protected override void Execute(
            NotifyUsersCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            var eventTypeId = this.notificationTypeResolver.GetIdentifier(command.Data.NotificationType);

            var notifications = command.UserIdentifiers.Select(userId => new Notification
            {
                CreatedDate = DateTime.UtcNow,
                NotificationTypeId = eventTypeId,
                UserId = userId,
                Data = command.Data
            }).ToArray();

            var notificationsCollection = this.mongoDatabaseFactory.CreateDatabase().GetCollection<Notification>("notifications");
            notificationsCollection.CreateIndex("EventTypeId");
            notificationsCollection.CreateIndex("UserId");
            notificationsCollection.CreateIndex("Data._t");
            notificationsCollection.CreateIndex("IsRead");
            notificationsCollection.InsertBatch(notifications);

            this.eventBus.Raise(new NotificationsAddedEvent
            {
                Notifications = notifications.Select(n => new NotificationViewModel
                {
                    Data = n.Data,
                    NotificationType = command.Data.NotificationType,
                    NotificationId = n.Id.ToString(),
                    UserId = n.UserId,
                    IsRead = n.IsRead
                }).ToList()
            });
        }
    }
}