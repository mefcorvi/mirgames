namespace MirGames.Domain.Chat.UserSettings
{
    using System;

    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The use enter to send message settings.
    /// </summary>
    internal sealed class UseWebSocketSetting : IUserSettingHandler
    {
        /// <inheritdoc />
        public string SettingKey
        {
            get { return "UseWebSocket"; }
        }

        /// <inheritdoc />
        public object FromViewModel(object value)
        {
            return Convert.ToBoolean(value);
        }

        /// <inheritdoc />
        public object ToViewModel(object value)
        {
            return value ?? true;
        }
    }
}
