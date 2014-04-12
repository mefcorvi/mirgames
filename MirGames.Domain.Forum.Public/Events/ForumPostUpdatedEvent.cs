// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostUpdatedEvent.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Events
{
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic updated.
    /// </summary>
    public sealed class ForumPostUpdatedEvent : Event
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <inheritdoc />
        protected override string EventType
        {
            get { return "Forum.ForumPostUpdated"; }
        }
    }
}