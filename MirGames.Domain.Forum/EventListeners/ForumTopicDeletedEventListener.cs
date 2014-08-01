// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicDeletedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.EventListeners
{
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Notifications.Commands;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about new topic created.
    /// </summary>
    internal sealed class ForumTopicDeletedEventListener : EventListenerBase<ForumTopicDeletedEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicDeletedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumTopicDeletedEventListener(ICommandProcessor commandProcessor)
        {
            Contract.Requires(commandProcessor != null);

            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicDeletedEvent @event)
        {
            Contract.Requires(@event != null);

            this.commandProcessor.Execute(new RemovePermissionsCommand { EntityId = @event.TopicId, EntityType = "ForumTopic" });
            this.commandProcessor.Execute(new RemoveNotificationsCommand
            {
                Filter =
                    n =>
                    (n is NewForumAnswerNotification && ((NewForumAnswerNotification)n).TopicId == @event.TopicId)
                    || (n is NewForumTopicNotification && ((NewForumTopicNotification)n).TopicId == @event.TopicId),
            });
        }
    }
}