// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetEventLogQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Tools.Queries
{
    using System;

    using MirGames.Domain.Tools.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topic by identifier.
    /// </summary>
    [Api]
    public class GetEventLogQuery : Query<EventLogViewModel>
    {
        /// <summary>
        /// Gets or sets the type of the log.
        /// </summary>
        public EventLogType? LogType { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets the to date.
        /// </summary>
        public DateTime? To { get; set; }
    }
}