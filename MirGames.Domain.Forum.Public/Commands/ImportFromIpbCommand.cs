namespace MirGames.Domain.Forum.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Converts the posts from IPB forum.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public sealed class ImportFromIpbCommand : Command
    {
    }
}