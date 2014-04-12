// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ActivationRequestedNotification.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Notifications
{
    using MirGames.Domain.Users;
    using MirGames.Infrastructure.Notifications;

    /// <summary>
    /// The account activation message.
    /// </summary>
    public sealed class ActivationRequestedNotification : Notification
    {
        /// <inheritdoc />
        public override string Title
        {
            get { return Localization.ActivationRequestedNotification_Title; }
        }

        /// <inheritdoc />
        public override string Body
        {
            get { return Localization.ActivationRequestedNotification_Body; }
        }

        /// <summary>
        /// Gets or sets the activation code.
        /// </summary>
        public string ActivationCode { get; set; }

        /// <summary>
        /// Gets or sets the activation link.
        /// </summary>
        public string ActivationLink { get; set; }
    }
}
