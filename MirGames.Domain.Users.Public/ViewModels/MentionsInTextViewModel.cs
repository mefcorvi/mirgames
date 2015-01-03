// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MentionsInTextViewModel.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MentionsInTextViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MentionsInTextViewModel"/> class.
        /// </summary>
        public MentionsInTextViewModel()
        {
            this.Users = Enumerable.Empty<AuthorViewModel>();
        }

        /// <summary>
        /// Gets or sets the user identifiers.
        /// </summary>
        public IEnumerable<AuthorViewModel> Users { get; set; }

        /// <summary>
        /// Gets or sets the transformed text.
        /// </summary>
        public string TransformedText { get; set; }
    }
}
