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
