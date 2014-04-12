﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositoryAccessMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Services.Git.Entities;

    /// <summary>
    /// Mapping of the project RepositoryAccess.
    /// </summary>
    internal class RepositoryAccessMap : EntityTypeConfiguration<RepositoryAccess>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryAccessMap"/> class.
        /// </summary>
        public RepositoryAccessMap()
        {
            this.ToTable("git_repositories_access");
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("access_id");
            this.Property(t => t.AccessLevel).HasColumnName("level");
            this.Property(t => t.RepositoryId).HasColumnName("repository_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}
