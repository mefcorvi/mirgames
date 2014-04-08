namespace MirGames.Domain.Wip.ViewModels
{
    using System;

    using MirGames.Domain.Users.ViewModels;

    public sealed class WipProjectCommitViewModel
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }
    }
}