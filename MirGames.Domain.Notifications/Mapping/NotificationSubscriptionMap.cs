// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotificationSubscriptionMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Notifications.Entities;

    internal class NotificationSubscriptionMap : EntityTypeConfiguration<NotificationSubscription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSubscriptionMap"/> class.
        /// </summary>
        public NotificationSubscriptionMap()
        {
            this.ToTable("notification_subscriptions");
            this.HasKey(t => t.SubscriptionId);

            this.Property(t => t.SubscriptionId).HasColumnName("subscription_id");
            this.Property(t => t.EntityId).HasColumnName("entity_id");
            this.Property(t => t.EntityTypeId).HasColumnName("entity_type_id");
            this.Property(t => t.EventTypeId).HasColumnName("event_type_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}