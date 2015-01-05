// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewCommitNotificationDetailsViewModel.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.Notifications
{
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Users.ViewModels;

    public sealed class NewCommitNotificationDetailsViewModel : NotificationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the commit identifier.
        /// </summary>
        public string CommitId { get; set; }

        /// <summary>
        /// Gets or sets the project alias.
        /// </summary>
        public string ProjectAlias { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the project title.
        /// </summary>
        public string ProjectTitle { get; set; }

        /// <inheritdoc />
        public override string NotificationType { get; set; }
    }
}
