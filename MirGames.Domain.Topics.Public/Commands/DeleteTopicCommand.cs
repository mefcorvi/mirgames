// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DeleteTopicCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Commands
{
    using System.ComponentModel;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The add new topic command.
    /// </summary>
    [Authorize(Roles = "User")]
    [Api("Удаляет топик блога")]
    public class DeleteTopicCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        [Description("Идентификатор топика")]
        public int TopicId { get; set; }
    }
}
