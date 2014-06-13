// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UpdateForumPostCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Posts new reply in the topic.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api]
    public sealed class UpdateForumPostCommand : Command
    {
        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        [Required]
        public IEnumerable<int> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the topics tags.
        /// </summary>
        public string TopicsTags { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        [Required]
        public int PostId { get; set; }
    }
}