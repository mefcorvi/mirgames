﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostNewCommentCommand.cs">
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
    /// Adds new comment to the topic and returns an identifier of the command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api("Добавляет новый комментарий в топик", ReturnDescription = "Идентификатор комментария", ExecutionInterval = 1000)]
    public class PostNewCommentCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        [Required]
        public IEnumerable<int> Attachments { get; set; }
    }
}
