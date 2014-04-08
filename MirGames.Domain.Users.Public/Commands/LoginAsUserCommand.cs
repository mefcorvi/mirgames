namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Sign-in as the specified user without using of password or login. Only for administrators.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    public class LoginAsUserCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}