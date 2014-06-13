namespace MirGames.Domain.Acl.Public.Queries
{
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns true whether action is allowed.
    /// </summary>
    public sealed class IsActionAllowedQuery : SingleItemQuery<bool>
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        [Required]
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        [Required]
        public string ActionName { get; set; }
    }
}
