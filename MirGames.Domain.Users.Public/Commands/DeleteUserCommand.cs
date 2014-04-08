namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Deletes the specified user.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class DeleteUserCommand : Command
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}
