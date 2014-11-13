// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The topic view model.
    /// </summary>
    public class TopicViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the blog.
        /// </summary>
        public BlogViewModel Blog { get; set; }

        /// <summary>
        /// Gets or sets the comments count.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public IEnumerable<CommentViewModel> Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether topic can be edited.
        /// </summary>
        public bool CanBeEdited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether can be commented.
        /// </summary>
        public bool CanBeCommented { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get { return this.TagsList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
        }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether topics should be shown on main.
        /// </summary>
        public bool ShowOnMain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is micro topic.
        /// </summary>
        public bool IsMicroTopic { get; set; }
    }
}