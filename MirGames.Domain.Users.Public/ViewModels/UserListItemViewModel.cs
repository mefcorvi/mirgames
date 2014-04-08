namespace MirGames.Domain.Users.ViewModels
{
    using System;

    /// <summary>
    /// The user.
    /// </summary>
    public sealed class UserListItemViewModel
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
        /// Gets or sets the registration date.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the last visit.
        /// </summary>
        public DateTime LastVisit { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the user rating.
        /// </summary>
        public int UserRating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is online.
        /// </summary>
        public bool IsOnline { get; set; }
    }
}