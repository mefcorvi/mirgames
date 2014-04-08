namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Requests password restoring.
    /// </summary>
    [Api]
    public class RequestPasswordRestoreCommand : Command
    {
        /// <summary>
        /// Gets or sets the email original login.
        /// </summary>
        public string EmailOrLogin { get; set; }

        /// <summary>
        /// Gets or sets the new password hash.
        /// </summary>
        public string NewPasswordHash { get; set; }
    }
}