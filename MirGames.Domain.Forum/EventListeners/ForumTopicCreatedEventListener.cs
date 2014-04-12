// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicCreatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.EventListeners
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about new topic created.
    /// </summary>
    internal sealed class ForumTopicCreatedEventListener : EventListenerBase<ForumTopicCreatedEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicCreatedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumTopicCreatedEventListener(Lazy<ICommandProcessor> commandProcessor)
        {
            Contract.Requires(commandProcessor != null);

            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicCreatedEvent @event)
        {
            Contract.Requires(@event != null);

            this.commandProcessor.Value.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
            this.commandProcessor.Value.Execute(new MarkTopicAsUnreadForUsersCommand { TopicId = @event.TopicId, TopicDate = @event.CreationDate });
            this.commandProcessor.Value.Execute(new MarkTopicAsReadCommand { TopicId = @event.TopicId });
        }
    }
}