// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AddNewBlogCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Commands
{
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    public sealed class AddNewBlogCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        [Required]
        public string EntityType { get; set; }
    }
}