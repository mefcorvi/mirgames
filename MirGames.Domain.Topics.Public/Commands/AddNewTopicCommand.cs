// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AddNewTopicCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The add new topic command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api("Добавляет новый топик в блог", ReturnDescription = "Идентификатор топика", ExecutionInterval = 1000)]
    public class AddNewTopicCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tutorial.
        /// </summary>
        public bool IsTutorial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is repost.
        /// </summary>
        public bool IsRepost { get; set; }

        /// <summary>
        /// Gets or sets the source author.
        /// </summary>
        [MaxLength(128)]
        public string SourceAuthor { get; set; }

        /// <summary>
        /// Gets or sets the source link.
        /// </summary>
        [Url]
        public string SourceLink { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IEnumerable<int> Attachments { get; set; }
    }
}
