// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicRepliedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumTopicRepliedEventListener : EventListenerBase<ForumTopicRepliedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicRepliedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public ForumTopicRepliedEventListener(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(queryProcessor != null);

            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicRepliedEvent @event)
        {
            var users = this.queryProcessor.Process(new GetUsersIdentifiersQuery()).Except(new[] { @event.AuthorId.GetValueOrDefault() });

            this.commandProcessor.Execute(new NotifyUsersCommand
            {
                NotificationTemplate = new NewForumAnswerNotification { TopicId = @event.TopicId, PostId = @event.PostId, ForumId = @event.ForumId },
                UserIdentifiers = users.ToArray()
            });

            this.commandProcessor.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
        }
    }
}