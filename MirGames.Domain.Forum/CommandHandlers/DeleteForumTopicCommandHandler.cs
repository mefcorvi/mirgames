// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DeleteForumTopicCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class DeleteForumTopicCommandHandler : CommandHandler<DeleteForumTopicCommand>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteForumTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public DeleteForumTopicCommandHandler(
            IReadContextFactory readContextFactory,
            IWriteContextFactory writeContextFactory,
            IEventBus eventBus)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);

            this.readContextFactory = readContextFactory;
            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override void Execute(DeleteForumTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            ForumTopic topic;
            IEnumerable<int> topicReaders;

            using (var readContext = this.readContextFactory.Create())
            {
                topic = readContext.Query<ForumTopic>().FirstOrDefault(t => t.TopicId == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("ForumTopic", command.TopicId);
                }

                authorizationManager.EnsureAccess(principal, "Delete", "ForumTopic", topic.TopicId);

                topicReaders = readContext.Query<ForumTopicRead>()
                    .Where(t => (t.StartTopicId >= command.TopicId && t.EndTopicId <= command.TopicId))
                    .Select(t => t.UserId)
                    .ToArray()
                    .Where(userId => userId.HasValue)
                    .Select(userId => userId.GetValueOrDefault())
                    .ToList();
            }

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<ForumTopic>().Attach(topic);
                writeContext.Set<ForumTopic>().Remove(topic);
                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new ForumTopicReadEvent { TopicId = command.TopicId, ExcludedUsers = topicReaders });
            this.eventBus.Raise(new ForumTopicDeletedEvent { TopicId = topic.TopicId });
        }
    }
}