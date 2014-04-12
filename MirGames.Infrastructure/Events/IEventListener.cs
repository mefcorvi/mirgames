// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IEventListener.cs">
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
    using System.Collections.Generic;

    /// <summary>
    /// Listens the game events.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Gets the supported event types.
        /// </summary>
        IEnumerable<string> SupportedEventTypes { get; }

        /// <summary>
        /// Determines whether this instance can process the specified game event.
        /// </summary>
        /// <param name="event">The game event.</param>
        /// <returns>True whether this instance can process the specified game event.</returns>
        bool CanProcess(Event @event);

        /// <summary>
        /// Processes the specified game event.
        /// </summary>
        /// <param name="event">The game event.</param>
        void Process(Event @event);
    }
}