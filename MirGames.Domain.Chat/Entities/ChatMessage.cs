// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatMessage.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The chat message.
    /// </summary>
    internal sealed class ChatMessage
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        public int MessageId { get; set; }

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
        /// Gets or sets the created date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
