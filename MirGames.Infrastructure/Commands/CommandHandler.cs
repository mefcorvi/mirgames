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
        void ICommandHandler.Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command != null);
            Contract.Requires(principal != null);
            this.Execute((T)command, principal, authorizationManager);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public abstract void Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);

        /// <summary>
        /// Gets the event log message.
        /// </summary>
        /// <returns>The event log message.</returns>
        public virtual string GetEventLogMessage()
        {
            return null;
        }
    }

    /// <summary>
    /// The command handler.
    /// </summary>
    /// <typeparam name="T">Type of command.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class CommandHandler<T, TResult> : ICommandHandler<TResult> where T : Command<TResult>
    {
        /// <inheritdoc />
        public Type CommandType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        void ICommandHandler.Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command != null);
            Contract.Requires(principal != null);
            this.Execute((T)command, principal, authorizationManager);
        }

        /// <inheritdoc />
        object ICommandHandlerWithResult.Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command != null);
            Contract.Requires(principal != null);
            return this.Execute((T)command, principal, authorizationManager);
        }

        /// <inheritdoc />
        TResult ICommandHandler<TResult>.Execute(Command<TResult> command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
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
        public abstract TResult Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }
}