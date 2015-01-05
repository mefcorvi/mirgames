// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewCommitNotification.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.Notifications
{
    using MirGames.Domain.Notifications.ViewModels;

    public sealed class NewCommitNotification : NotificationData
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
        /// Gets or sets the comiteer identifier.
        /// </summary>
        public int ComiteerId { get; set; }

        /// <inheritdoc />
        public override string NotificationType
        {
            get { return "Wip.NewCommit"; }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NewCommitNotification
            {
                CommitId = this.CommitId,
                ComiteerId = this.ComiteerId,
                IsRead = this.IsRead,
                NotificationDate = this.NotificationDate,
                NotificationTypeId = this.NotificationTypeId,
                ProjectAlias = this.ProjectAlias,
                UserId = this.UserId
            };
        }
    }
}
