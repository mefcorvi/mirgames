namespace MirGames.Domain.Topics.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Runs the topics re-indexation.
    /// </summary>
    [Authorize(Roles = "User")]
    public class ReindexTopicsCommand : Command
    {
    }
}