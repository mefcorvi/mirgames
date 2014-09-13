// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPost.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The forum post.
    /// </summary>
    internal sealed class ForumPost
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the source text.
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the author login.
        /// </summary>
        public string AuthorLogin { get; set; }

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
        /// Gets or sets the topic.
        /// </summary>
        public ForumTopic Topic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current post if first post in the topic.
        /// </summary>
        public bool IsStartPost { get; set; }
    }
}