namespace MirGames.Domain.Users.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// The current user view model.
    /// </summary>
    public sealed class CurrentUserViewModel
    {
        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user is activated.
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public Dictionary<string, object> Settings { get; set; }
    }
}