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
