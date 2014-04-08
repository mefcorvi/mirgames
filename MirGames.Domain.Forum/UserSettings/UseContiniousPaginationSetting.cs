namespace MirGames.Domain.Forum.UserSettings
{
    using System;

    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The use continious pagination setting.
    /// </summary>
    internal sealed class UseContiniousPaginationSetting : IUserSettingHandler
    {
        /// <inheritdoc />
        public string SettingKey
        {
            get { return "ForumContiniousPagination"; }
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
