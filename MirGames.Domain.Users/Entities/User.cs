// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="User.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Entities
{
    using System;
    using System.Collections.Generic;

    using MirGames.Infrastructure;

    using Newtonsoft.Json;

    /// <summary>
    /// The settings.
    /// </summary>
    internal sealed class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.Settings = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the hash of the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the mail.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public IDictionary<string, object> Settings { get; private set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public string SettingsJson
        {
            get { return JsonConvert.SerializeObject(this.Settings); }
            set { this.Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(value); }
        }

        /// <summary>
        /// Gets or sets the registration IP.
        /// </summary>
        public string RegistrationIP { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the activation date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the last comment date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? LastCommentDate { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public int UserRating { get; set; }

        /// <summary>
        /// Gets or sets the user count vote.
        /// </summary>
        public int UserCountVote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is activated.
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        /// Gets or sets the user activation key.
        /// </summary>
        public string UserActivationKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the gender of the user.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the about.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the photo URL.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [notice new topic].
        /// </summary>
        public bool NoticeNewTopic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [notice new comment].
        /// </summary>
        public bool NoticeNewComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [notice new talk].
        /// </summary>
        public bool NoticeNewTalk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [notice reply comment].
        /// </summary>
        public bool NoticeReplyComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [notice new friend].
        /// </summary>
        public bool NoticeNewFriend { get; set; }

        /// <summary>
        /// Gets or sets the user settings time zone.
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the time zone time span.
        /// </summary>
        public TimeZoneInfo TimezoneInfo
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(this.Timezone); }
            set { this.Timezone = value.Id; }
        }

        /// <summary>
        /// Gets or sets the related user administrator record.
        /// </summary>
        public UserAdministrator UserAdministrator { get; set; }

        /// <summary>
        /// Gets or sets the last visit.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastVisitDate { get; set; }

        /// <summary>
        /// Gets or sets the password salt.
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Creates the new user.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user.</returns>
        public static User Create(string login, string email, string password)
        {
            return new User
                {
                    Login = login,
                    Mail = email,
                    Password = password,
                    RegistrationDate = DateTime.UtcNow,
                    Gender = "other",
                    LastVisitDate = DateTime.UtcNow
                };
        }
    }
}
