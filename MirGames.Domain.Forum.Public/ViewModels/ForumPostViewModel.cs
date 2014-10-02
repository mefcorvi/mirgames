// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostViewModel.cs">
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
    /// The forum posts list item.
    /// </summary>
    public class ForumPostViewModel
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the author IP.
        /// </summary>
        public string AuthorIP { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether post is hidden.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        public int? ForumId { get; set; }

        /// <summary>
        /// Gets or sets the forum alias.
        /// </summary>
        public string ForumAlias { get; set; }

        /// <summary>
        /// Gets or sets the votes rating.
        /// </summary>
        public int VotesRating { get; set; }

        /// <summary>
        /// Gets or sets the current user vote.
        /// </summary>
        public int? UserVote { get; set; }
    }
}