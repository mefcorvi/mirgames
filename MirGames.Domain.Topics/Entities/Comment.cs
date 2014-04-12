// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Comment.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The comment entity.
    /// </summary>
    internal sealed class Comment
    {
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user login.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the source text.
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user IP.
        /// </summary>
        public string UserIP { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        public Topic Topic { get; set; }
    }
}
