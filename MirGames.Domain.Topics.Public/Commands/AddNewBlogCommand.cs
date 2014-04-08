namespace MirGames.Domain.Topics.Commands
{
    using MirGames.Infrastructure.Commands;

    public sealed class AddNewBlogCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}