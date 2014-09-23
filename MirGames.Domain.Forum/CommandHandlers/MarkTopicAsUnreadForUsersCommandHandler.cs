// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkTopicAsUnreadForUsersCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Marks topic as unread for the all users.
    /// </summary>
    internal sealed class MarkTopicAsUnreadForUsersCommandHandler : CommandHandler<MarkTopicAsUnreadForUsersCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkTopicAsUnreadForUsersCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public MarkTopicAsUnreadForUsersCommandHandler(IWriteContextFactory writeContextFactory, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override void Execute(MarkTopicAsUnreadForUsersCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            IEnumerable<ForumTopicUnread> newUnreads;

            using (var writeContext = this.writeContextFactory.Create())
            {
                var oldReads = writeContext
                    .Set<ForumTopicRead>()
                    .Where(t => command.TopicId >= t.StartTopicId && command.TopicId <= t.EndTopicId)
                    .ToList();

                var users = new List<int>();

                foreach (ForumTopicRead oldRead in oldReads)
                {
                    if (oldRead.UserId != null)
                    {
                        users.Add(oldRead.UserId.Value);
                    }

                    if (oldRead.StartTopicId == command.TopicId)
                    {
                        oldRead.StartTopicId = oldRead.StartTopicId + 1;
                        continue;
                    }
                    
                    if (oldRead.EndTopicId == command.TopicId)
                    {
                        oldRead.EndTopicId = oldRead.EndTopicId - 1;
                        continue;
                    }

                    var newRead = new ForumTopicRead
                        {
                            StartTopicId = command.TopicId + 1,
                            EndTopicId = oldRead.EndTopicId,
                            UserId = oldRead.UserId
                        };

                    oldRead.EndTopicId = command.TopicId - 1;

                    if (newRead.StartTopicId <= newRead.EndTopicId)
                    {
                        writeContext.Set<ForumTopicRead>().Add(newRead);
                    }
                }

                foreach (var oldRead in oldReads)
                {
                    if (oldRead.StartTopicId > oldRead.EndTopicId)
                    {
                        writeContext.Set<ForumTopicRead>().Remove(oldRead);
                    }
                }

                var unreads = writeContext.Set<ForumTopicUnread>().Where(u => u.TopicId == command.TopicId && users.Contains(u.UserId));
                writeContext.Set<ForumTopicUnread>().RemoveRange(unreads);

                newUnreads = users.Select(
                    u => new ForumTopicUnread
                        {
                            TopicId = command.TopicId,
                            UnreadDate = command.TopicDate,
                            UserId = u
                        }).ToList();

                writeContext.Set<ForumTopicUnread>().AddRange(newUnreads);
                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new ForumTopicUnreadEvent
            {
                TopicId = command.TopicId,
                UserIdentifiers = newUnreads.Select(n => n.UserId).Where(userId => userId != principal.GetUserId()).ToList(),
                ExcludedUsers = new[] { principal.GetUserId().GetValueOrDefault() }
            });
        }
    }
}