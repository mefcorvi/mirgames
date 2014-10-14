// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="LoginAsUserCommand.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Commands
{
    using System.ComponentModel;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Sign-in as the specified user without using of password or login. Only for administrators.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    [Api("Позволяет залогиниться в систему от имени другого пользователя")]
    public class LoginAsUserCommand : Command<string>
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        [Description("Идентификатор пользователя")]
        public int UserId { get; set; }
    }
}