// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AttachmentViewModel.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.ViewModels
{
    using System;

    /// <summary>
    /// The attachment view model.
    /// </summary>
    public sealed class AttachmentViewModel
    {
        /// <summary>
        /// Gets or sets the attachment unique identifier.
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the attachment URL.
        /// </summary>
        public string AttachmentUrl { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Gets or sets the entity unique identifier.
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attachment is image.
        /// </summary>
        public bool IsImage { get; set; }
    }
}
