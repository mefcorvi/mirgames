// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserEntityExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    using MirGames.Domain.Users.ViewModels;

    public static class UserEntityExtensions
    {
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="user">The user.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting value.</returns>
        public static T GetSetting<T>(this CurrentUserViewModel user, string key, T defaultValue = default(T))
        {
            if (user == null)
            {
                return defaultValue;
            }

            object value;

            if (user.Settings.TryGetValue(key, out value))
            {
                return (T)value;
            }

            return defaultValue;
        }
    }
}