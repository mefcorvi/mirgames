namespace MirGames.Domain.Forum.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Marks the specified topic as read.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class MarkAllTopicsAsReadCommand : Command
    {
    }
}