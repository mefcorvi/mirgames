// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WipProjectFileViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.ViewModels
{
    using System;
    using System.IO;

    public class WipProjectFileViewModel
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the commit identifier.
        /// </summary>
        public string CommitId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preview.
        /// </summary>
        public bool IsPreview { get; set; }
    }
}