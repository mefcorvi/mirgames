namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Removes tag for the current online user.
    /// </summary>
    [DisableTracing]
    [Api]
    public class RemoveOnlineUserTagCommand : Command
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }
    }
}
