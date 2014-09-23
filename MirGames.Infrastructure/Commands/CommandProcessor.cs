// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommandProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Transactions;

    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The command processor.
    /// </summary>
    internal sealed class CommandProcessor : ICommandProcessor
    {
        /// <summary>
        /// The claims principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> claimsPrincipalProvider;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The command handler decorators.
        /// </summary>
        private readonly IEnumerable<ICommandHandlerDecorator> commandHandlerDecorators;

        /// <summary>
        /// The command handlers.
        /// </summary>
        private readonly Lazy<Dictionary<Type, ICommandHandler>> commandHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor" /> class.
        /// </summary>
        /// <param name="commandHandlers">The command handlers.</param>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="commandHandlerDecorators">The command handler decorators.</param>
        public CommandProcessor(
            Lazy<IEnumerable<ICommandHandler>> commandHandlers,
            Func<ClaimsPrincipal> claimsPrincipalProvider,
            IAuthorizationManager authorizationManager,
            IEnumerable<ICommandHandlerDecorator> commandHandlerDecorators)
        {
            Contract.Requires(commandHandlers != null);
            Contract.Requires(claimsPrincipalProvider != null);

            this.claimsPrincipalProvider = claimsPrincipalProvider;
            this.authorizationManager = authorizationManager;
            this.commandHandlerDecorators = commandHandlerDecorators.OrderBy(d => d.Order).ToList();
            this.commandHandlers = new Lazy<Dictionary<Type, ICommandHandler>>(() => commandHandlers.Value.ToDictionary(c => c.CommandType));
        }

        /// <inheritdoc />
        public void Execute(Command command)
        {
            Contract.Requires(command != null);
            this.ExecuteWithResult(command);
        }

        /// <inheritdoc />
        public T Execute<T>(Command<T> command)
        {
            Contract.Requires(command != null);
            var result = this.ExecuteWithResult(command);
            return result != null ? (T)result : default(T);
        }

        /// <inheritdoc />
        public object ExecuteWithResult(Command command)
        {
            Contract.Requires(command != null);
            var commandType = command.GetType();
            var principal = this.claimsPrincipalProvider.Invoke();

            if (this.commandHandlers.Value.ContainsKey(commandType))
            {
                var commandHandler = this.commandHandlers.Value[commandType];
                commandHandler = this.commandHandlerDecorators.Aggregate(commandHandler, (current, handlerDecorator) => handlerDecorator.Decorate(current));

                try
                {
                    using (var transaction = this.CreateTransactionScope())
                    {
                        var commandResult = commandHandler.Execute(command, principal, this.authorizationManager);
                        transaction.Complete();

                        return commandResult;
                    }
                }
                catch (CommandProcessorException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new CommandProcessorException(command, e);
                }
            }

            throw new CommandProcessorException(command, "Command handler for commands of type " + commandType.FullName + " have not been found.");
        }

        /// <summary>
        /// Creates the transaction scope.
        /// </summary>
        /// <returns>The transaction scope.</returns>
        private TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }
    }
}
