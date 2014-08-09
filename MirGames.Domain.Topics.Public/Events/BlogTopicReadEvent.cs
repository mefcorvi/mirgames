// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BlogTopicReadEvent.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.Events
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when some of topics has been read.
    /// </summary>
    public class BlogTopicReadEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public IEnumerable<int> UserIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the excluded users.
        /// </summary>
        public IEnumerable<int> ExcludedUsers { get; set; } 

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        protected override string EventType
        {
            get { return "Topics.BlogTopicRead"; }
        }
    }
}