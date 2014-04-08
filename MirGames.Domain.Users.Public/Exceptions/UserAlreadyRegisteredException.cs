namespace MirGames.Domain.Users.Exceptions
{
    using System;

    /// <summary>
    /// Exception that raised when user with the same login or email exists.
    /// </summary>
    public class UserAlreadyRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAlreadyRegisteredException"/> class.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="mail">The mail.</param>
        public UserAlreadyRegisteredException(string login, string mail)
        {
            this.Login = login;
            this.Mail = mail;
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Gets the mail.
        /// </summary>
        public string Mail { get; private set; }
    }
}