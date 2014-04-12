// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetChatMessagesQuery.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.Queries
{
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the chat messages.
    /// </summary>
    [Api]
    public sealed class GetChatMessagesQuery : Query<ChatMessageViewModel>
    {
        /// <summary>
        /// Gets or sets the maximum identifier of received messages.
        /// </summary>
        public int? LastIndex { get; set; }

        /// <summary>
        /// Gets or sets the minimum identifier of received messages.
        /// </summary>
        public int? FirstIndex { get; set; }
    }
}
