namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Requests password restoring.
    /// </summary>
    public class RestorePasswordCommand : Command
    {
        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public string SecretKey { get; set; }
    }
}