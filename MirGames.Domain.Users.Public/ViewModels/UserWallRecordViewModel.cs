namespace MirGames.Domain.Users.ViewModels
{
    using System;

    /// <summary>
    /// The user wall record view model.
    /// </summary>
    public sealed class UserWallRecordViewModel
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the date add.
        /// </summary>
        public DateTime DateAdd { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}