namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Marks a command as command that required authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// The split array of roles.
        /// </summary>
        private string[] rolesSplit;

        /// <summary>
        /// The split array of users.
        /// </summary>
        private string[] usersSplit;

        /// <summary>
        /// The roles.
        /// </summary>
        private string roles;

        /// <summary>
        /// The users.
        /// </summary>
        private string users;

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public string Roles
        {
            get
            {
                return this.roles;
            }

            set
            {
                this.roles = value;
                this.rolesSplit = SplitPropertyValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public string Users
        {
            get
            {
                return this.users;
            }

            set
            {
                this.users = value;
                this.usersSplit = SplitPropertyValue(value);
            }
        }

        /// <inheritdoc />
        public override bool IsValid(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            bool isAuthorized = false;

            if (this.Roles != null)
            {
                isAuthorized |= this.rolesSplit.Any(principal.IsInRole);
            }

            if (this.Users != null)
            {
                isAuthorized |= this.usersSplit.Any(u => principal.Identity.Name == u);
            }

            return isAuthorized;
        }

        /// <summary>
        /// Splits the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The array of split properties.</returns>
        private static string[] SplitPropertyValue(string value)
        {
            return value.Split(new[] { " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}