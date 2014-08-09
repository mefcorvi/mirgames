// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommentCreatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the comment created event.
    /// </summary>
    internal sealed class CommentCreatedEventListener : EventListenerBase<CommentCreatedEvent>
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
        /// Initializes a new instance of the <see cref="CommentCreatedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public CommentCreatedEventListener(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(queryProcessor != null);

            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(CommentCreatedEvent @event)
        {
            var userIdentifiers = this.queryProcessor.Process(new GetUsersIdentifiersQuery()).Except(new[] { @event.AuthorId });
            this.commandProcessor.Execute(new NotifyUsersCommand
            {
                UserIdentifiers = userIdentifiers.ToArray(),
                Data = new NewTopicCommentNotification
                {
                    TopicId = @event.TopicId,
                    CommentId = @event.CommentId
                }
            });
        }
    }
}