// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostUploadProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.Services
{
    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class ChatMessageUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("chatMessage");
        }

        /// <inheritdoc />
        public bool IsValid(string filePath)
        {
            return true;
        }

        /// <inheritdoc />
        public void Process(string filePath)
        {
        }
    }
}
