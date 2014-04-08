namespace MirGames.Domain.Users.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Saves the account settings.
    /// </summary>
    [Api]
    public class SaveAccountSettingsCommand : Command
    {
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public Dictionary<string, object> Settings { get; set; }
    }
}