// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetCurrentUserQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.Services;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    using Newtonsoft.Json;

    /// <summary>
    /// Handles the GetCurrentUserQuery.
    /// </summary>
    internal sealed class GetCurrentUserQueryHandler : SingleItemQueryHandler<GetCurrentUserQuery, CurrentUserViewModel>
    {
        /// <summary>
        /// The user setting handlers.
        /// </summary>
        private readonly IEnumerable<IUserSettingHandler> userSettingHandlers;

        /// <summary>
        /// The avatar provider.
        /// </summary>
        private readonly IAvatarProvider avatarProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentUserQueryHandler" /> class.
        /// </summary>
        /// <param name="userSettingHandlers">The user setting handlers.</param>
        /// <param name="avatarProvider">The avatar provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetCurrentUserQueryHandler(IEnumerable<IUserSettingHandler> userSettingHandlers, IAvatarProvider avatarProvider, IReadContextFactory readContextFactory)
        {
            Contract.Requires(userSettingHandlers != null);
            Contract.Requires(avatarProvider != null);
            Contract.Requires(readContextFactory != null);

            this.userSettingHandlers = userSettingHandlers;
            this.avatarProvider = avatarProvider;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override CurrentUserViewModel Execute(GetCurrentUserQuery query, ClaimsPrincipal principal)
        {
            var userId = principal.GetUserId().GetValueOrDefault();

            UserInfo user;
            using (var readContext = this.readContextFactory.Create())
            {
                user = readContext
                    .Query<User>()
                    .Where(u => u.Id == userId)
                    .Select(
                        u =>
                        new UserInfo
                        {
                            AvatarUrl = u.AvatarUrl,
                            Id = u.Id,
                            Login = u.Login,
                            Name = u.Name,
                            SettingsJson = u.SettingsJson,
                            Timezone = u.Timezone,
                            IsActivated = u.IsActivated,
                            Mail = u.Mail
                        })
                    .FirstOrDefault();
            }

            if (user == null)
            {
                return null;
            }

            var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(user.SettingsJson);
            var settingsViewModel = new Dictionary<string, object>();

            foreach (IUserSettingHandler settingHandler in this.userSettingHandlers)
            {
                string key = settingHandler.SettingKey;
                settingsViewModel[key] = settingHandler.ToViewModel(settings.ContainsKey(key) ? settings[key] : null);
            }

            settingsViewModel["TimeZone"] = user.Timezone;

            return new CurrentUserViewModel
                {
                    AvatarUrl = user.AvatarUrl ?? this.avatarProvider.GetAvatarUrl(user.Mail, user.Login),
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name,
                    Settings = settingsViewModel,
                    IsActivated = user.IsActivated,
                    TimeZone = user.Timezone
                };
        }

        /// <summary>
        /// The user info.
        /// </summary>
        private sealed class UserInfo
        {
            /// <summary>
            /// Gets or sets the avatar URL.
            /// </summary>
            public string AvatarUrl { get; set; }

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the login.
            /// </summary>
            public string Login { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the settings json.
            /// </summary>
            public string SettingsJson { get; set; }

            /// <summary>
            /// Gets or sets the timezone.
            /// </summary>
            public string Timezone { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is activated.
            /// </summary>
            public bool IsActivated { get; set; }

            /// <summary>
            /// Gets or sets the mail.
            /// </summary>
            public string Mail { get; set; }
        }
    }
}