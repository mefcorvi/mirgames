namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Security.Claims;

    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Attribute that could be used to add some validation logic to the command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ValidationAttribute : Attribute
    {
        /// <summary>
        /// Determines whether the specified command is valid.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <returns>
        ///   <c>true</c> if the specified command is valid; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsValid(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager);
    }
}