namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Sets the user avatar.
    /// </summary>
    [Api]
    public class SetUserAvatarCommand : Command
    {
        /// <summary>
        /// Gets or sets the avatar attachment unique identifier.
        /// </summary>
        public int AvatarAttachmentId { get; set; }
    }
}