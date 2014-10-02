namespace MirGames.Domain.Users.UserSettings
{
    using System;

    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The theme settings.
    /// </summary>
    internal sealed class ThemeSetting : IUserSettingHandler
    {
        /// <inheritdoc />
        public string SettingKey
        {
            get { return "Theme"; }
        }

        /// <inheritdoc />
        public object FromViewModel(object value)
        {
            return Convert.ToString(value);
        }

        /// <inheritdoc />
        public object ToViewModel(object value)
        {
            return value;
        }
    }
}