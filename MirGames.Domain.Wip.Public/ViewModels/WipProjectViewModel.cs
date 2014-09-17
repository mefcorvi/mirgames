// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WipProjectViewModel.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;

    /// <summary>
    /// The WIP project view model.
    /// </summary>
    public class WipProjectViewModel
    {
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public int ProjectId { get; set; }

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

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the logo URL.
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the votes count.
        /// </summary>
        public int VotesCount { get; set; }

        /// <summary>
        /// Gets or sets the votes.
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Gets or sets the followers count.
        /// </summary>
        public int FollowersCount { get; set; }

        /// <summary>
        /// Gets or sets the repository URL.
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// Gets or sets the type of the repository.
        /// </summary>
        public string RepositoryType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can edit.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create bug.
        /// </summary>
        public bool CanCreateBug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create task.
        /// </summary>
        public bool CanCreateTask { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create feature.
        /// </summary>
        public bool CanCreateFeature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is repository private.
        /// </summary>
        public bool CanReadRepository { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create blog topic.
        /// </summary>
        public bool CanCreateBlogTopic { get; set; }

        /// <summary>
        /// Gets or sets the last commit message.
        /// </summary>
        public string LastCommitMessage { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is repository private.
        /// </summary>
        public bool IsRepositoryPrivate { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }
    }
}