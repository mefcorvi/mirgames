// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicViewModel.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The forum topic view model.
    /// </summary>
    public class ForumTopicViewModel
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the author IP.
        /// </summary>
        public string AuthorIp { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the start post.
        /// </summary>
        public ForumPostsListItemViewModel StartPost { get; set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get { return this.TagsList.Split(','); }
        }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this topic can be answered by current user.
        /// </summary>
        public bool CanBeAnswered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this topic can be edited by current user.
        /// </summary>
        public bool CanBeEdited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this topic can be deleted by current user.
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether topic is read.
        /// </summary>
        public bool IsRead { get; set; }
    }
}