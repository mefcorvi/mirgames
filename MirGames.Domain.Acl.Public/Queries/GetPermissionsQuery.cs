namespace MirGames.Domain.Acl.Public.Queries
{
    using MirGames.Domain.Acl.Public.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the permissions.
    /// </summary>
    public sealed class GetPermissionsQuery : Query<PermissionViewModel>
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
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter by user identifier.
        /// </summary>
        public bool FilterByUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter by entity identifier.
        /// </summary>
        public bool FilterByEntityId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter by entity type.
        /// </summary>
        public bool FilterByEntityType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter by action name.
        /// </summary>
        public bool FilterByActionName { get; set; }
    }
}