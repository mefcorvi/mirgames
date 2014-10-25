// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommandProcessorException.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Exception
{
    using System;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Exception thrown whether command executing have been failed.
    /// </summary>
    public class CommandProcessorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessorException" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="message">The message.</param>
        public CommandProcessorException(Command command, string message = @"Execution of command has failed")
            : base(message)
        {
            this.Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessorException"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommandProcessorException(Command command, Exception innerException)
            : base("Execution of command has failed", innerException)
        {
            this.Command = command;
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public Command Command { get; set; }
    }
}