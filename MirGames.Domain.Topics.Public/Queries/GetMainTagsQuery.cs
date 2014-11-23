// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetMainTagsQuery.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns tags that are should be shown on the main page.
    /// </summary>
    [Api]
    public sealed class GetMainTagsQuery : Query<TagViewModel>
    {
        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on main.
        /// </summary>
        public bool ShowOnMain { get; set; }

        /// <summary>
        /// Gets or sets the is tutorial.
        /// </summary>
        public bool? IsTutorial { get; set; }

        /// <summary>
        /// Gets or sets the micro topic.
        /// </summary>
        public bool? IsMicroTopic { get; set; }
    }
}