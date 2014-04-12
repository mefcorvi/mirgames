// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicTagMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class TopicTagMap : EntityTypeConfiguration<TopicTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicTagMap"/> class.
        /// </summary>
        public TopicTagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TagText).IsRequired().HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("topic_tag");
            this.Property(t => t.Id).HasColumnName("tag_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.TagText).HasColumnName("tag_text");
        }
    }
}