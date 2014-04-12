// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicUnreadMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class ForumTopicUnreadMap : EntityTypeConfiguration<ForumTopicUnread>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicUnreadMap"/> class.
        /// </summary>
        public ForumTopicUnreadMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("forum_topic_unread");
            this.Property(t => t.Id).HasColumnName("topic_unread_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UnreadDate).HasColumnName("unread_date");
        }
    }
}