// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicsUnreadCountQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns count of unread forum topics.
    /// </summary>
    internal sealed class GetForumTopicsUnreadCountQueryHandler : SingleItemQueryHandler<GetForumTopicsUnreadCountQuery, int>
    {
        /// <inheritdoc />
        public override int Execute(IReadContext readContext, GetForumTopicsUnreadCountQuery query, ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return 0;
            }

            int userId = principal.GetUserId().GetValueOrDefault();

            var topicReadItems = readContext.Query<ForumTopicRead>().Where(t => t.UserId == userId);

            int totalCount = readContext.Query<ForumTopic>().Count();
            int readCount = readContext.Query<ForumTopic>().SelectMany(
                topic => topicReadItems,
                (topic, topicRead) => new
                    {
                        topic,
                        topicRead
                    }).Where(
                        t => t.topicRead.UserId == userId && t.topic.TopicId >= t.topicRead.StartTopicId
                             && t.topic.TopicId <= t.topicRead.EndTopicId).Select(
                                 t => t.topic).Count();

            return totalCount - readCount;
        }
    }
}
