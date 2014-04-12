// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EventLogMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Tools.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Tools.Entities;

    /// <summary>
    /// The topic tag map.
    /// </summary>
    internal class EventLogMap : EntityTypeConfiguration<EventLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogMap"/> class.
        /// </summary>
        public EventLogMap()
        {
            this.ToTable("eventLog");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.Login).HasMaxLength(255).HasColumnName("login");
            this.Property(t => t.EventLogType).IsRequired().HasColumnName("eventType");
            this.Property(t => t.Message).IsRequired().HasMaxLength(1024).HasColumnName("message");
            this.Property(t => t.Date).HasColumnName("date");
            this.Property(t => t.Details).HasColumnName("details");
        }
    }
}