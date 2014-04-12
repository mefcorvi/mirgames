// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DateTimeHelper.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace System.Web.Mvc
{
    using System.Security.Claims;

    using MirGames.Domain.Security;

    /// <summary>
    /// Date Time helper.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns the relative date.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// The string representation of the relative date.
        /// </returns>
        public static string RelativeDate(this HtmlHelper helper, DateTime date, string format = "{0} {1} назад")
        {
            const int Second = 1;
            const int Minute = 60 * Second;
            const int Hour = 60 * Minute;
            const int Day = 24 * Hour;
            const int Month = 30 * Day;

            var ts = DateTime.UtcNow - date;
            var delta = ts.TotalSeconds;

            if (delta < 0)
            {
                return "прямо сейчас";
            }

            if (delta < 1 * Minute)
            {
                return ts.Seconds.Pluralize("секунду", "секунды", "секунд", format);
            }

            if (delta < 2 * Minute)
            {
                return "минуту назад";
            }

            if (delta < 45 * Minute)
            {
                return ts.Minutes.Pluralize("минуту", "минуты", "минут", format);
            }

            if (delta < 90 * Minute)
            {
                return "час назад";
            }

            if (delta < 24 * Hour)
            {
                return ts.Hours.Pluralize("час", "часа", "часов", format);
            }

            if (delta < 48 * Hour)
            {
                return "вчера";
            }

            if (delta < 30 * Day)
            {
                return ts.Days.Pluralize("день", "дня", "дней", format);
            }

            if (delta < 12 * Month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months.Pluralize("месяц", "месяца", "месяцев", format);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years.Pluralize("год", "года", "лет", format);
        }

        /// <summary>
        /// Converts the UTC to the user's time-zone.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The users date.</returns>
        public static DateTime UserDate(this DateTime date)
        {
            var timeZone = ClaimsPrincipal.Current.GetTimeZone() ?? TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);
        }

        /// <summary>
        /// Converts the UTC to the user's time-zone.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The users date.</returns>
        public static string Format(this DateTime date)
        {
            return string.Format("{0:dd.MM.yy HH:mm}", date.UserDate());
        }
    }
}