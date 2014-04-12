// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="FavoriteMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The favorite map.
    /// </summary>
    internal class FavoriteMap : EntityTypeConfiguration<Favorite>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteMap"/> class.
        /// </summary>
        public FavoriteMap()
        {
            // Primary Key
            this.HasKey(t => t.TargetId);

            // Properties
            this.Property(t => t.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TargetType).HasMaxLength(65532);

            this.Property(t => t.Tags).IsRequired().HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("prefix_favourite");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.TargetId).HasColumnName("target_id");
            this.Property(t => t.TargetType).HasColumnName("target_type");
            this.Property(t => t.TargetPublish).HasColumnName("target_publish");
            this.Property(t => t.Tags).HasColumnName("tags");
        }

        #endregion
    }
}