namespace MirGames.Domain.Users.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Transforms mentions of users into links.
    /// </summary>
    public sealed class TransformMentionsCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}
