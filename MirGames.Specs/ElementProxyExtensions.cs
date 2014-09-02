// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ElementProxyExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Specs
{
    using FluentAutomation;
    using FluentAutomation.Exceptions;

    /// <summary>
    /// Extensions of Element Proxy.
    /// </summary>
    public static class ElementProxyExtensions
    {
        /// <summary>
        /// Determines whether element exists.
        /// </summary>
        /// <param name="elementProxy">The element proxy.</param>
        /// <returns>True whether element exists.</returns>
        public static bool Exists(this ElementProxy elementProxy)
        {
            try
            {
                return elementProxy.Element != null;
            }
            catch (FluentException)
            {
                return false;
            }
        }
    }
}