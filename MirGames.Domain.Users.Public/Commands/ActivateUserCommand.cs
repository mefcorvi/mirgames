namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The activate user command.
    /// </summary>
    public class ActivateUserCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string ActivationKey { get; set; }
    }
}