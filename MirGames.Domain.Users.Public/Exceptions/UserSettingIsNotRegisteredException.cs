// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserSettingIsNotRegisteredException.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Exceptions
{
    using System;

    /// <summary>
    /// Raised when the specified setting has not been registered.
    /// </summary>
    public class UserSettingIsNotRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingIsNotRegisteredException"/> class.
        /// </summary>
        /// <param name="settingsKey">The settings key.</param>
        public UserSettingIsNotRegisteredException(string settingsKey) : base(string.Format("The specified settings with key \"{0}\" hasn't been registered", settingsKey))
        {
            this.SettingsKey = settingsKey;
        }

        /// <summary>
        /// Gets the settings key.
        /// </summary>
        public string SettingsKey { get; private set; }
    }
}