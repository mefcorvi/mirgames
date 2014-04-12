// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicReadMap.cs">
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
    internal class ForumTopicReadMap : EntityTypeConfiguration<ForumTopicRead>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicReadMap"/> class.
        /// </summary>
        public ForumTopicReadMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("forum_topic_read");
            this.Property(t => t.Id).HasColumnName("topic_read_id");
            this.Property(t => t.StartTopicId).HasColumnName("start_topic_id");
            this.Property(t => t.EndTopicId).HasColumnName("end_topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}