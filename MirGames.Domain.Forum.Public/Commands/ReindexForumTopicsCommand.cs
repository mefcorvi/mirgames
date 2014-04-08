namespace MirGames.Domain.Forum.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Re-indexes the forum topic.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public sealed class ReindexForumTopicsCommand : Command
    {
    }
}