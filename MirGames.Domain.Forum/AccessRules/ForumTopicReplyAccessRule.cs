// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicReplyAccessRule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to post replying.
    /// </summary>
    internal sealed class ForumTopicReplyAccessRule : AccessRule<ForumTopic>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Reply"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ForumTopic resource)
        {
            return principal.IsInRole("TopicsAuthor");
        }
    }
}