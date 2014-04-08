namespace MirGames.Domain.Users.Commands
{
    using System;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Saves user profile.
    /// </summary>
    [Api]
    public class SaveUserProfileCommand : Command
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the birth day.
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the career.
        /// </summary>
        public string Career { get; set; }

        /// <summary>
        /// Gets or sets the about.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets or sets the git hub link.
        /// </summary>
        public string GitHubLink { get; set; }

        /// <summary>
        /// Gets or sets the bit bucket link.
        /// </summary>
        public string BitBucketLink { get; set; }

        /// <summary>
        /// Gets or sets the habrahabr link.
        /// </summary>
        public string HabrahabrLink { get; set; }
    }
}
