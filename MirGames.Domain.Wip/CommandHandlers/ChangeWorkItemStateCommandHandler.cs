// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChangeWorkItemStateCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Change Work Item state command.
    /// </summary>
    internal sealed class ChangeWorkItemStateCommandHandler : CommandHandler<ChangeWorkItemStateCommand, WorkItemState>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeWorkItemStateCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public ChangeWorkItemStateCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override WorkItemState Execute(ChangeWorkItemStateCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var workItem = writeContext.Set<ProjectWorkItem>()
                                           .FirstOrDefault(t => t.WorkItemId == command.WorkItemId);

                if (workItem == null)
                {
                    throw new ItemNotFoundException("WorkItem", command.WorkItemId);
                }

                authorizationManager.EnsureAccess(principal, "ChangeStateWorkItem", "Project", workItem.ProjectId);

                workItem.State = command.State ?? this.GetNextState(workItem.State);
                writeContext.SaveChanges();

                return workItem.State;
            }
        }

        /// <summary>
        /// Gets the next state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The next state.</returns>
        private WorkItemState GetNextState(WorkItemState state)
        {
            switch (state)
            {
                case WorkItemState.Undefined:
                    return WorkItemState.Open;
                case WorkItemState.Open:
                    return WorkItemState.Active;
                case WorkItemState.Closed:
                    return WorkItemState.Open;
                case WorkItemState.Active:
                    return WorkItemState.Closed;
                case WorkItemState.Queued:
                    return WorkItemState.Open;
                case WorkItemState.Removed:
                    return WorkItemState.Open;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}