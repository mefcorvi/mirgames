// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkAllTopicsAsReadCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class MarkAllTopicsAsReadCommandHandler : CommandHandler<MarkAllTopicsAsReadCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkAllTopicsAsReadCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public MarkAllTopicsAsReadCommandHandler(IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(MarkAllTopicsAsReadCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            using (var writeContext = this.writeContextFactory.Create())
            {
                var lastTopic = writeContext.Set<ForumTopic>().OrderByDescending(t => t.TopicId).FirstOrDefault();
                var firstTopic = writeContext.Set<ForumTopic>().OrderBy(t => t.TopicId).FirstOrDefault();

                if (lastTopic == null || firstTopic == null)
                {
                    return;
                }

                authorizationManager.EnsureAccess(principal, "MarkAsRead", new ForumTopic());

                var oldReadTopics = writeContext.Set<ForumTopicRead>().Where(u => u.UserId == userId);
                writeContext.Set<ForumTopicRead>().RemoveRange(oldReadTopics);
                writeContext.Set<ForumTopicRead>().Add(
                    new ForumTopicRead
                        {
                            UserId = userId,
                            StartTopicId = firstTopic.TopicId,
                            EndTopicId = lastTopic.TopicId
                        });

                var oldUnreadTopics = writeContext.Set<ForumTopicUnread>().Where(u => u.UserId == userId);
                writeContext.Set<ForumTopicUnread>().RemoveRange(oldUnreadTopics);

                writeContext.SaveChanges();
            }
        }
    }
}