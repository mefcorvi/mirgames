namespace MirGames.Domain.Users.Services
{
    /// <summary>
    /// Handles the user settings.
    /// </summary>
    public interface IUserSettingHandler
    {
        /// <summary>
        /// Gets the setting key.
        /// </summary>
        string SettingKey { get; }

        /// <summary>
        /// Converts value from the view model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The transformed object.</returns>
        object FromViewModel(object value);

        /// <summary>
        /// Converts saved value to the view model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The view model.</returns>
        object ToViewModel(object value);
    }
}
