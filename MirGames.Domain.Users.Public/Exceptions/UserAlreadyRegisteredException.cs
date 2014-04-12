// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserAlreadyRegisteredException.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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