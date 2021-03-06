﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectWorkItem.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.Entities
{
    using System;

    using MirGames.Domain.Wip.ViewModels;

    /// <summary>
    /// The project work item.
    /// </summary>
    internal sealed class ProjectWorkItem
    {
        /// <summary>
        /// Gets or sets the work item unique identifier.
        /// </summary>
        public int WorkItemId { get; set; }

        /// <summary>
        /// Gets or sets the internal identifier.
        /// </summary>
        public int InternalId { get; set; }

        /// <summary>
        /// Gets or sets the project unique identifier.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        public int AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public WorkItemState State { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        public WorkItemType ItemType { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        public int? DurationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan? Duration
        {
            get { return this.DurationInSeconds == null ? (TimeSpan?)null : TimeSpan.FromSeconds(this.DurationInSeconds.Value); }
            set { this.DurationInSeconds = value == null ? null : (int?)value.Value.TotalSeconds; }
        }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }
    }
}
