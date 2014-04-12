// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SaveUserProfileCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Commands
{
    using System;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Saves user profile.
    /// </summary>
    [Api]
    public class SaveUserProfileCommand : Command
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the birth day.
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the career.
        /// </summary>
        public string Career { get; set; }

        /// <summary>
        /// Gets or sets the about.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets or sets the git hub link.
        /// </summary>
        public string GitHubLink { get; set; }

        /// <summary>
        /// Gets or sets the bit bucket link.
        /// </summary>
        public string BitBucketLink { get; set; }

        /// <summary>
        /// Gets or sets the habrahabr link.
        /// </summary>
        public string HabrahabrLink { get; set; }
    }
}
