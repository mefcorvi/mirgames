// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DeleteTopicCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class DeleteTopicCommandHandler : CommandHandler<DeleteTopicCommand>
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
        /// Initializes a new instance of the <see cref="DeleteTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public DeleteTopicCommandHandler(IWriteContextFactory writeContextFactory, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(DeleteTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            Topic topic;
            
            using (var writeContext = this.writeContextFactory.Create())
            {
                topic = writeContext.Set<Topic>().SingleOrDefault(t => t.Id == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                authorizationManager.EnsureAccess(principal, "Delete", topic);

                writeContext.Set<Topic>().Remove(topic);

                var comments = writeContext.Set<Comment>().Where(c => c.TopicId == topic.Id).ToList();
                if (comments.Any())
                {
                    writeContext.Set<Comment>().RemoveRange(comments);
                }

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new TopicDeletedEvent { TopicId = topic.Id });
        }
    }
}