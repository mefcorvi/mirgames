// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostNewForumTopicCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Posts new command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api("Создаёт новый топик форума", ReturnDescription = "Идентификатор созданного топика")]
    public sealed class PostNewForumTopicCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the forum alias.
        /// </summary>
        [Required]
        [Description("Алиас форума")]
        public string ForumAlias { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        [Description("Заголовок поста")]
        [MinLength(1)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        [Description("Текст топика")]
        [MinLength(1)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [Required]
        [Description("Тэги")]
        [MinLength(1)]
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        [Required]
        [Description("Приложенные файлы")]
        public IEnumerable<int> Attachments { get; set; }
    }
}
