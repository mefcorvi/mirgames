namespace MirGames.Domain.Acl.Public.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Removes all permissions that are match the conditions.
    /// </summary>
    public sealed class RemovePermissionsCommand : Command
    {
        public int? UserId { get; set; }

        public int? EntityId { get; set; }

        public string EntityType { get; set; }

        public string ActionName { get; set; }
    }
}