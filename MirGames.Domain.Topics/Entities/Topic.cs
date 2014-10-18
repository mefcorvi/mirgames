// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Topic.cs">
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
    /// The topic entity.
    /// </summary>
    internal sealed class Topic
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the type of the topic.
        /// </summary>
        public string TopicType { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the edit date.
        /// </summary>
        public DateTime? EditDate { get; set; }

        /// <summary>
        /// Gets or sets the user ip.
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published draft.
        /// </summary>
        public bool IsPublishedDraft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published index.
        /// </summary>
        public bool IsPublishedIndex { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the count vote.
        /// </summary>
        public int CountVote { get; set; }

        /// <summary>
        /// Gets or sets the count vote up.
        /// </summary>
        public int CountVoteUp { get; set; }

        /// <summary>
        /// Gets or sets the count vote down.
        /// </summary>
        public int CountVoteDown { get; set; }

        /// <summary>
        /// Gets or sets the count vote abstain.
        /// </summary>
        public int CountVoteAbstain { get; set; }

        /// <summary>
        /// Gets or sets the count read.
        /// </summary>
        public int CountRead { get; set; }

        /// <summary>
        /// Gets or sets the count comment.
        /// </summary>
        public int CountComment { get; set; }

        /// <summary>
        /// Gets or sets the count favorite.
        /// </summary>
        public int CountFavorite { get; set; }

        /// <summary>
        /// Gets or sets the cut text.
        /// </summary>
        public string CutText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether commenting is forbidden.
        /// </summary>
        public bool ForbidComment { get; set; }

        /// <summary>
        /// Gets or sets the text hash.
        /// </summary>
        public string TextHash { get; set; }

        /// <summary>
        /// Gets or sets the is repost.
        /// </summary>
        public bool? IsRepost { get; set; }

        /// <summary>
        /// Gets or sets the source author.
        /// </summary>
        public string SourceAuthor { get; set; }

        /// <summary>
        /// Gets or sets the source link.
        /// </summary>
        public string SourceLink { get; set; }

        /// <summary>
        /// Gets or sets the is tutorial.
        /// </summary>
        public bool? IsTutorial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether topic should be shown on main.
        /// </summary>
        public bool ShowOnMain { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public TopicContent Content { get; set; }
    }
}
