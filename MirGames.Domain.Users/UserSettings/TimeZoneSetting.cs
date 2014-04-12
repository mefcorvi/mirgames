// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TimeZoneSetting.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.UserSettings
{
    using System;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The Time Zone settings.
    /// </summary>
    internal sealed class TimeZoneSetting : IUserSettingHandler
    {
        /// <inheritdoc />
        public string SettingKey
        {
            get { return "TimeZone"; }
        }

        /// <inheritdoc />
        public object FromViewModel(object value)
        {
            var timeZone = Convert.ToString(value);

            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            }
            catch (Exception)
            {
                throw new ItemNotFoundException("TimeZone", timeZone);
            }

            return timeZone;
        }

        /// <inheritdoc />
        public object ToViewModel(object value)
        {
            return value;
        }
    }
}
