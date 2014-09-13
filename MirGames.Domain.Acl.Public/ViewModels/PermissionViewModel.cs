namespace MirGames.Domain.Acl.Public.ViewModels
{
    using System;

    using MirGames.Infrastructure;

    public class PermissionViewModel
    {
        /// <summary>
        /// Gets or sets the permission identifier.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the entity type identifier.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        public int? ActionId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is denied.
        /// </summary>
        public bool IsDenied { get; set; }

        /// <summary>
        /// Gets or sets the expired date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}