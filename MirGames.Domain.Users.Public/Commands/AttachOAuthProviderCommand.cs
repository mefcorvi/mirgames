namespace MirGames.Domain.Users.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Attaches provider to the current user.
    /// </summary>
    public class AttachOAuthProviderCommand : Command
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the provider user unique identifier.
        /// </summary>
        public string ProviderUserId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public IDictionary<string, string> Data { get; set; }
    }
}