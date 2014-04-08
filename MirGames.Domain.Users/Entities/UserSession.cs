namespace MirGames.Domain.Users.Entities
{
    using System;

    /// <summary>
    /// The user session.
    /// </summary>
    internal class UserSession
    {
        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the creation IP.
        /// </summary>
        public string CreationIP { get; set; }

        /// <summary>
        /// Gets or sets the last visit IP.
        /// </summary>
        public string LastVisitIP { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the last date.
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}
