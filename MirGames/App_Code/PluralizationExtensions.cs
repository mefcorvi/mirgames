// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PluralizationExtensions.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace System.Web.Mvc
{
    /// <summary>
    /// Pluralizes the specified number.
    /// </summary>
    public static class PluralizationExtensions
    {
        /// <summary>
        /// Pluralizes the specified number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="singleItem">The single item.</param>
        /// <param name="fourItems">The four items.</param>
        /// <param name="fiveItems">The five items.</param>
        /// <param name="format">The format.</param>
        /// <returns>The pluralized result.</returns>
        public static string Pluralize(this int number, string singleItem, string fourItems, string fiveItems, string format = "{1}")
        {
            int originalNumber = number;
            string pluralization;
            number = number % 100;

            if (number >= 11 && number <= 19)
            {
                pluralization = fiveItems;
            }
            else
            {
                number = number % 10;
                switch (number)
                {
                    case 1:
                        pluralization = singleItem;
                        break;
                    case 2:
                    case 3:
                    case 4:
                        pluralization = fourItems;
                        break;
                    default:
                        pluralization = fiveItems;
                        break;
                }
            }

            return string.Format(format, originalNumber, pluralization);
        }
    }
}