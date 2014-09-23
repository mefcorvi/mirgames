// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The command handler.
    /// </summary>
    /// <typeparam name="T">Type of command.</typeparam>
    public abstract class CommandHandler<T> : ICommandHandler where T : Command
    {
        /// <inheritdoc />
        public Type CommandType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        object ICommandHandler.Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command != null);
            Contract.Requires(principal != null);
            this.Execute((T)command, principal, authorizationManager);
            return null;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        protected abstract void Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }

    /// <summary>
    /// The command handler.
    /// </summary>
    /// <typeparam name="T">Type of command.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class CommandHandler<T, TResult> : ICommandHandler where T : Command<TResult>
    {
        /// <inheritdoc />
        public Type CommandType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        object ICommandHandler.Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command != null);
            Contract.Requires(principal != null);
            return this.Execute((T)command, principal, authorizationManager);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <returns>The result.</returns>
        protected abstract TResult Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }
}