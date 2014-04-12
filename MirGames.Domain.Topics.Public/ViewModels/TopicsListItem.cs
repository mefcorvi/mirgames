// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicsListItem.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// Represents an one item in the topic list.
    /// </summary>
    public class TopicsListItem
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the short text.
        /// </summary>
        public string ShortText { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the comments count.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets the tags set.
        /// </summary>
        public IEnumerable<string> TagsSet
        {
            get { return this.Tags.Split(','); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether item can be edited.
        /// </summary>
        public bool CanBeEdited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item can be commented.
        /// </summary>
        public bool CanBeCommented { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }
    }
}