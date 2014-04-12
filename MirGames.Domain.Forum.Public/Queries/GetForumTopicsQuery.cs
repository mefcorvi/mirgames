// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicsQuery.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topic by identifier.
    /// </summary>
    public class GetForumTopicsQuery : Query<ForumTopicsListItemViewModel>
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only unread topics should be returned.
        /// </summary>
        public bool OnlyUnread { get; set; }
    }
}