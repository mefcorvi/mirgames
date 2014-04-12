// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EnumerableExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions of IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the specified set to the collection.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The collection.</returns>
        public static ICollection<T> EnsureCollection<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is ICollection<T>)
            {
                return (ICollection<T>)enumerable;
            }

            return enumerable.ToList();
        }

        /// <summary>
        /// Applies the callback to the each of items.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="callback">The callback.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> callback)
        {
            foreach (var item in items)
            {
                callback(item);
            }
        }

        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="items">The items.</param>
        /// <returns>The sequence.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, params T[] items)
        {
            return enumerable.Concat((IEnumerable<T>)items);
        }

        /// <summary>
        /// Determines whether enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>True whether the specified enumerable is null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is string)
            {
                return string.IsNullOrEmpty(enumerable as string);
            }

            return enumerable == null || !enumerable.Any();
        }
    }
}