// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ICommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Security.Claims;

    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The command handler.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        Type CommandType { get; }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        void Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }

    /// <summary>
    /// The command handler which returns the result.
    /// </summary>
    public interface ICommandHandlerWithResult : ICommandHandler
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <returns>The result.</returns>
        new object Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }

    /// <summary>
    /// The command handler.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ICommandHandler<TResult> : ICommandHandlerWithResult
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <returns>
        /// The result.
        /// </returns>
        TResult Execute(Command<TResult> command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }
}
