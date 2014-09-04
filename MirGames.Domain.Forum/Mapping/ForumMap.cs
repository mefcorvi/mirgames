// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The forum mapping.
    /// </summary>
    internal sealed class ForumMap : EntityTypeConfiguration<Forum>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumMap"/> class.
        /// </summary>
        public ForumMap()
        {
            this.ToTable("forum_forums");

            this.HasKey(t => t.ForumId);

            this.Property(t => t.ForumId).HasColumnName("forum_id");
            this.Property(t => t.Title).IsRequired().HasMaxLength(1024).HasColumnName("title");
            this.Property(t => t.Description).IsRequired().HasColumnName("description");
            this.Property(t => t.IsRetired).HasColumnName("retired");
            this.Property(t => t.Alias).IsRequired().HasColumnName("alias").HasMaxLength(50);
        }
    }
}