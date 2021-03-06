﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CreateNewWipProjectCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.Commands
{
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Creates the new project.
    /// </summary>
    [Api]
    public sealed class CreateNewWipProjectCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-z0-9\-_]{3,}$")]
        [MaxLength(255)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the type of the repository.
        /// </summary>
        public string RepositoryType { get; set; }

        /// <summary>
        /// Gets or sets the logo attachment identifier.
        /// </summary>
        public int LogoAttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        [Required]
        public int[] Attachments { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        [Required]
        public string ShortDescription { get; set; }
    }
}
