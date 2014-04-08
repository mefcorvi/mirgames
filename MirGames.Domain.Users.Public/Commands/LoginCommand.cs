namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The login command.
    /// </summary>
    [DisableTracing]
    public class LoginCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the email or login.
        /// </summary>
        public string EmailOrLogin { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}