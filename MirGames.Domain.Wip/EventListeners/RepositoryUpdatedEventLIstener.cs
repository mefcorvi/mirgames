// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositoryUpdatedEventLIstener.cs">
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
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Services.Git.Public.Events;

    internal sealed class RepositoryUpdatedEventLIstener : EventListenerBase<RepositoryUpdatedEvent>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryUpdatedEventLIstener"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public RepositoryUpdatedEventLIstener(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Process(RepositoryUpdatedEvent @event)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var projects = writeContext
                    .Set<Project>()
                    .Where(p => p.RepositoryType == "git" && p.RepositoryId == @event.RepositoryId);

                projects.ForEach(p => p.UpdatedDate = DateTime.UtcNow);

                writeContext.SaveChanges();
            }
        }
    }
}
