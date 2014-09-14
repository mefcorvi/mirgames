// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopic.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Entities
{
    using System;
    using System.Collections.Generic;

    using MirGames.Infrastructure;

    /// <summary>
    /// The forum topic.
    /// </summary>
    internal sealed class ForumTopic
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the forum identifier.
        /// </summary>
        public int? ForumId { get; set; }

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
        public string AuthorIp { get; set; }

        /// <summary>
        /// Gets or sets the last post author unique identifier.
        /// </summary>
        public int? LastPostAuthorId { get; set; }

        /// <summary>
        /// Gets or sets the last post author login.
        /// </summary>
        public string LastPostAuthorLogin { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the posts count.
        /// </summary>
        public int PostsCount { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        public ICollection<ForumPost> Posts { get; set; }
    }
}