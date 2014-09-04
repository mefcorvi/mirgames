// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.ViewModels
{
    using System;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The forum view model.
    /// </summary>
    public class ForumViewModel
    {
        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retired.
        /// </summary>
        public bool IsRetired { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the last author.
        /// </summary>
        public AuthorViewModel LastAuthor { get; set; }

        /// <summary>
        /// Gets or sets the last post title.
        /// </summary>
        public string LastTopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the last post identifier.
        /// </summary>
        public int? LastTopicId { get; set; }

        /// <summary>
        /// Gets or sets the last post date.
        /// </summary>
        public DateTime? LastPostDate { get; set; }

        /// <summary>
        /// Gets or sets the topics count.
        /// </summary>
        public int TopicsCount { get; set; }

        /// <summary>
        /// Gets or sets the posts count.
        /// </summary>
        public int PostsCount { get; set; }
    }
}