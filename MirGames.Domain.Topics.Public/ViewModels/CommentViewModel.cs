// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommentViewModel.cs">
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

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The comments.
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether comment can be edited.
        /// </summary>
        public bool CanBeEdited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether comment can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; }
    }
}