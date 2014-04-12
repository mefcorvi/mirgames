// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MirGamesException.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Exception
{
    using System;

    /// <summary>
    /// Base exception class.
    /// </summary>
    public class MirGamesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MirGamesException"/> class.
        /// </summary>
        public MirGamesException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MirGamesException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MirGamesException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MirGamesException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MirGamesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}