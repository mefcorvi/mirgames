// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositoryUpdatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.EventListeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Events;
    using MirGames.Domain.Wip.Notifications;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Events;

    internal sealed class RepositoryUpdatedEventListener : EventListenerBase<RepositoryUpdatedEvent>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryUpdatedEventListener" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public RepositoryUpdatedEventListener(
            IWriteContextFactory writeContextFactory,
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Process(RepositoryUpdatedEvent @event)
        {
            var users = this.queryProcessor
                            .Process(new GetUsersIdentifiersQuery())
                            .Except(new[] { @event.AuthorId })
                            .ToArray();

            List<NewCommitNotification> notifications;

            using (var writeContext = this.writeContextFactory.Create())
            {
                var projects = writeContext
                    .Set<Project>()
                    .Where(p => p.RepositoryType == "git" && p.RepositoryId == @event.RepositoryId)
                    .ToList();

                notifications = new List<NewCommitNotification>(projects.Count);

                foreach (var project in projects)
                {
                    var lastCommit = this.queryProcessor
                        .Process(new GetWipProjectCommitsQuery { Alias = project.Alias }, new PaginationSettings(0, 1))
                        .FirstOrDefault();

                    project.UpdatedDate = DateTime.UtcNow;
                    project.LastCommitMessage = lastCommit != null ? this.CutString(lastCommit.Message, 255) : null;

                    if (lastCommit != null)
                    {
                        notifications.Add(new NewCommitNotification
                        {
                            CommitId = lastCommit.Id,
                            ComiteerId = @event.AuthorId,
                            ProjectAlias = project.Alias
                        });

                        this.eventBus.Raise(new ProjectUpdatedEvent
                        {
                            ProjectId = project.ProjectId,
                            ProjectAlias = project.Alias
                        });
                    }
                }

                writeContext.SaveChanges();
            }

            foreach (var notification in notifications)
            {
                this.commandProcessor.Execute(new NotifyUsersCommand
                {
                    UserIdentifiers = users,
                    NotificationTemplate = notification
                });
            }
        }

        /// <summary>
        /// Cuts the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="length">The length.</param>
        /// <returns>The part of the string.</returns>
        private string CutString(string s, int length)
        {
            return s.Substring(0, Math.Min(s.Length, length));
        }
    }
}
