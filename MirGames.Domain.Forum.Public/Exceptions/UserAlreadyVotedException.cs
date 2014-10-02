// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserAlreadyVotedException.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Exceptions
{
    using System;

    /// <summary>
    /// Raised when user has already voted and trying to vote again.
    /// </summary>
    [Serializable]
    public sealed class UserAlreadyVotedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyVotedException"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postId">The post identifier.</param>
        public UserAlreadyVotedException(int userId, int postId)
        {
            this.UserId = userId;
            this.PostId = postId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets the post identifier.
        /// </summary>
        public int PostId { get; private set; }
    }
}