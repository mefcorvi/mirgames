namespace MirGames.Domain.Users.ViewModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The view model of author.
    /// </summary>
    public sealed class OnlineUserViewModel
    {
        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the date of first request of the user.
        /// </summary>
        public DateTime SessionDate { get; set; }

        /// <summary>
        /// Gets or sets the last request date.
        /// </summary>
        public DateTime LastRequestDate { get; set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public IEnumerable<string> Tags { get; internal set; }
    }
}