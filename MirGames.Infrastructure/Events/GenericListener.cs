// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GenericListener.cs">
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

    /// <summary>
    /// The generic game events listener.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    public sealed class GenericListener<T> : EventListenerBase<T> where T : Event, new()
    {
        /// <summary>
        /// The callback.
        /// </summary>
        private readonly Action<T> callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericListener{T}" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public GenericListener(Action<T> callback)
        {
            this.callback = callback;
        }

        /// <inheritdoc />
        public override void Process(T @event)
        {
            this.callback(@event);
        }
    }
}