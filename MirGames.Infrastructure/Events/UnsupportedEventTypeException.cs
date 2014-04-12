// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UnsupportedEventTypeException.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Events
{
    using System;

    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// Raised when event passed to the listener could not be converted to event supported by listener.
    /// </summary>
    public class UnsupportedEventTypeException : MirGamesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedEventTypeException"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnsupportedEventTypeException(Type targetType, Type expectedType, Exception innerException)
            : base(string.Format("Events of type {0} could not be processed by listeners of type {1}.", targetType, expectedType), innerException)
        {
        }
    }
}