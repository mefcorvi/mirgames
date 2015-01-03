// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewForumAnswerNotificationDetailsViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Notifications
{
    using System;

    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The details of the new forum answer notification.
    /// </summary>
    public sealed class NewForumAnswerNotificationDetailsViewModel : NotificationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the topic identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the post date.
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Gets or sets the post text.
        /// </summary>
        public string PostText { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the forum.
        /// </summary>
        public ForumViewModel Forum { get; set; }

        /// <inheritdoc />
        public override string NotificationType { get; set; }
    }
}