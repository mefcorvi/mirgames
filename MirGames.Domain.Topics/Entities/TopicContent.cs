// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicContent.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Entities
{
    /// <summary>
    /// The topic content entity.
    /// </summary>
    internal sealed class TopicContent
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic text.
        /// </summary>
        public string TopicText { get; set; }

        /// <summary>
        /// Gets or sets the topic text short.
        /// </summary>
        public string TopicTextShort { get; set; }

        /// <summary>
        /// Gets or sets the topic text source.
        /// </summary>
        public string TopicTextSource { get; set; }

        /// <summary>
        /// Gets or sets the topic extra.
        /// </summary>
        public string TopicExtra { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        public Topic Topic { get; set; }
    }
}
