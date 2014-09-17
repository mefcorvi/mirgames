// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkTopicAsVisitedCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class MarkTopicAsVisitedCommandHandler : CommandHandler<MarkTopicAsVisitedCommand>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkTopicAsVisitedCommandHandler" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        public MarkTopicAsVisitedCommandHandler(ICommandProcessor commandProcessor, IEventBus eventBus, IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(writeContextFactory != null);

            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(MarkTopicAsVisitedCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            using (var writeContext = this.writeContextFactory.Create())
            {
                var topic = writeContext.Set<ForumTopic>().FirstOrDefault(t => t.TopicId == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("ForumTopic", command.TopicId);
                }

                topic.Visits++;
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new RemoveNotificationsCommand
            {
                Filter =
                    n =>
                    (n is NewForumAnswerNotification && ((NewForumAnswerNotification)n).TopicId == command.TopicId)
                    || (n is NewForumTopicNotification && ((NewForumTopicNotification)n).TopicId == command.TopicId),
                UserIdentifiers = new[] { principal.GetUserId().GetValueOrDefault() }
            });

            this.eventBus.Raise(new ForumTopicReadEvent { TopicId = command.TopicId, UserIdentifiers = new[] { userId } });
        }
    }
}