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