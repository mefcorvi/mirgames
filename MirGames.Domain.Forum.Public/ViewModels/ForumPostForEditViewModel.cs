// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostForEditViewModel.cs">
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
    /// <summary>
    /// The forum posts list item.
    /// </summary>
    public class ForumPostForEditViewModel
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the topic tags.
        /// </summary>
        public string TopicTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can change title.
        /// </summary>
        public bool CanChangeTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can change tags.
        /// </summary>
        public bool CanChangeTags { get; set; }
    }
}