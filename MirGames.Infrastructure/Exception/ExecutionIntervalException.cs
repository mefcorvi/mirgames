// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ExecutionIntervalException.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.Exception
{
    using System;

    /// <summary>
    /// Raised when command couldn't be execution due to execution interval limit.
    /// </summary>
    [Serializable]
    public sealed class ExecutionIntervalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionIntervalException"/> class.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="executionInterval">The execution interval.</param>
        public ExecutionIntervalException(string commandType, TimeSpan executionInterval)
            : base(
                string.Format(
                    "Command {0} could not be executed because its execution interval is {1}ms",
                    commandType,
                    executionInterval.TotalMilliseconds))
        {
            this.CommandType = commandType;
            this.ExecutionInterval = executionInterval;
        }

        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        public string CommandType { get; private set; }

        /// <summary>
        /// Gets the execution interval.
        /// </summary>
        public TimeSpan ExecutionInterval { get; private set; }
    }
}