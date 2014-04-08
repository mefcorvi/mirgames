namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Adds tag for the current online user.
    /// </summary>
    [DisableTracing]
    [Api]
    public class AddOnlineUserTagCommand : Command
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the expiration time in milliseconds.
        /// </summary>
        public int? ExpirationTime { get; set; }
    }
}
