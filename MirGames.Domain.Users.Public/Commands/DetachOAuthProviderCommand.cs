namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Detaches provider from the current user.
    /// </summary>
    [Api]
    public class DetachOAuthProviderCommand : Command
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }
    }
}