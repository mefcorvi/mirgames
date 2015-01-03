namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Transforms mentions of users into links.
    /// </summary>
    public sealed class GetMentionsFromTextQuery : SingleItemQuery<MentionsInTextViewModel>
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}
