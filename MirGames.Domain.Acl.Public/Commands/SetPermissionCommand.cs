namespace MirGames.Domain.Acl.Public.Commands
{
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Grant the specified permission.
    /// </summary>
    public sealed class SetPermissionCommand : Command
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is denied.
        /// </summary>
        public bool IsDenied { get; set; }
    }
}
