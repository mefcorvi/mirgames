// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SaveAccountSettingsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Exceptions;
    using MirGames.Domain.Users.Security;
    using MirGames.Domain.Users.Services;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class SaveAccountSettingsCommandHandler : CommandHandler<SaveAccountSettingsCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The principal cache.
        /// </summary>
        private readonly IPrincipalCache principalCache;

        /// <summary>
        /// The user setting handlers.
        /// </summary>
        private readonly IDictionary<string, IUserSettingHandler> userSettingHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAccountSettingsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="principalCache">The principal cache.</param>
        /// <param name="userSettingHandlers">The user setting handlers.</param>
        public SaveAccountSettingsCommandHandler(IWriteContextFactory writeContextFactory, IPrincipalCache principalCache, IEnumerable<IUserSettingHandler> userSettingHandlers)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(principalCache != null);

            this.writeContextFactory = writeContextFactory;
            this.principalCache = principalCache;
            this.userSettingHandlers = userSettingHandlers.ToDictionary(setting => setting.SettingKey);
        }

        /// <inheritdoc />
        protected override void Execute(SaveAccountSettingsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int userId = principal.GetUserId().GetValueOrDefault();

            using (var writeContext = this.writeContextFactory.Create())
            {
                var user = writeContext.Set<User>().SingleOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    throw new ItemNotFoundException("User", userId);
                }

                var settings = user.Settings;

                foreach (KeyValuePair<string, object> keyValuePair in command.Settings)
                {
                    if (keyValuePair.Value == null)
                    {
                        continue;
                    }

                    if (!this.userSettingHandlers.ContainsKey(keyValuePair.Key))
                    {
                        throw new UserSettingIsNotRegisteredException(keyValuePair.Key);
                    }

                    var settingHandler = this.userSettingHandlers[keyValuePair.Key];
                    settings[keyValuePair.Key] = settingHandler.FromViewModel(keyValuePair.Value);
                }

                if (settings.ContainsKey("TimeZone"))
                {
                    user.Timezone = (string)settings["TimeZone"];
                    settings.Remove("TimeZone");
                }

                writeContext.SaveChanges();
            }

            this.principalCache.Remove(principal.GetSessionId());
        }
    }
}