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
    using System.Linq;

    using MirGames.Domain.Wip.Entities;
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
        /// Initializes a new instance of the <see cref="RepositoryUpdatedEventListener" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public RepositoryUpdatedEventListener(IWriteContextFactory writeContextFactory, IQueryProcessor queryProcessor)
        {
            this.writeContextFactory = writeContextFactory;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(RepositoryUpdatedEvent @event)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var projects = writeContext
                    .Set<Project>()
                    .Where(p => p.RepositoryType == "git" && p.RepositoryId == @event.RepositoryId);

                projects.ForEach(p =>
                {
                    var lastCommit = this.queryProcessor
                        .Process(new GetWipProjectCommitsQuery { Alias = p.Alias }, new PaginationSettings(0, 1))
                        .FirstOrDefault();

                    p.UpdatedDate = DateTime.UtcNow;
                    p.LastCommitMessage = lastCommit != null ? lastCommit.Message.Substring(0, 255) : null;
                });

                writeContext.SaveChanges();
            }
        }
    }
}
