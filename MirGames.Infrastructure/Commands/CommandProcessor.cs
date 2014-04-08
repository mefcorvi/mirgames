namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Transactions;

    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Logging;
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
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The command handlers.
        /// </summary>
        private readonly Lazy<Dictionary<Type, ICommandHandler>> commandHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor" /> class.
        /// </summary>
        /// <param name="commandHandlers">The command handlers.</param>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        /// <param name="eventLog">The event log.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="settings">The settings.</param>
        public CommandProcessor(
            Lazy<IEnumerable<ICommandHandler>> commandHandlers,
            Func<ClaimsPrincipal> claimsPrincipalProvider,
            IEventLog eventLog,
            IAuthorizationManager authorizationManager,
            ISettings settings)
        {
            Contract.Requires(commandHandlers != null);
            Contract.Requires(claimsPrincipalProvider != null);

            this.claimsPrincipalProvider = claimsPrincipalProvider;
            this.eventLog = eventLog;
            this.authorizationManager = authorizationManager;
            this.settings = settings;
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

            this.ValidateCommand(command, principal);

            if (this.commandHandlers.Value.ContainsKey(commandType))
            {
                var commandHandlerWithResult = this.commandHandlers.Value[commandType] as ICommandHandlerWithResult;

                if (commandHandlerWithResult != null)
                {
                    object commandResult = null;
                    this.ExecuteCommandWithTrace(() => commandResult = commandHandlerWithResult.Execute(command, principal, this.authorizationManager), command, commandType);
                    return commandResult;
                }

                var commandHandler = this.commandHandlers.Value[commandType];
                this.ExecuteCommandWithTrace(() => commandHandler.Execute(command, principal, this.authorizationManager), command, commandType);
                return true;
            }

            throw new CommandProcessorException(command, "Command handler for commands of type " + commandType.FullName + " have not been found.");
        }

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        private void ValidateCommand(Command command, ClaimsPrincipal principal)
        {
            var commandType = command.GetType();
            var attributes = commandType.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();

            if (!attributes.All(attribute => attribute.IsValid(command, principal, this.authorizationManager)))
            {
                throw new InvalidCommandException(command);
            }
        }

        /// <summary>
        /// Traces the command execution.
        /// </summary>
        /// <param name="commandAction">The command action.</param>
        /// <param name="command">The command.</param>
        /// <param name="commandType">Type of the command.</param>
        private void ExecuteCommandWithTrace(Action commandAction, Command command, Type commandType)
        {
            Contract.Requires(commandAction != null);
            Contract.Requires(command != null);

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                using (var transaction = this.CreateTransactionScope())
                {
                    commandAction();
                    transaction.Complete();
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

            sw.Stop();

            if (this.settings.GetValue("CommandBus.TraceEnabled", false))
            {
                var attributes = commandType.GetCustomAttributes(typeof(DisableTracingAttribute), true).Cast<DisableTracingAttribute>();

                if (attributes.Any())
                {
                    return;
                }

                this.eventLog.Log(
                    EventLogType.Verbose,
                    "CommandBus.Execute",
                    string.Format("Execution of command \"{0}\" completed in \"{1}\" ms", commandType.Name, sw.ElapsedMilliseconds),
                    command);
            }
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
