namespace MirGames.Infrastructure.UserSettings
{
    using System;

    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The theme settings.
    /// </summary>
    internal sealed class HeaderTypeSetting : IUserSettingHandler
    {
        /// <inheritdoc />
        public string SettingKey
        {
            get { return "HeaderType"; }
        }

        /// <inheritdoc />
        public object FromViewModel(object value)
        {
            return Convert.ToString(value);
        }

        /// <inheritdoc />
        public object ToViewModel(object value)
        {
            return value ?? "Fixed";
        }
    }
}