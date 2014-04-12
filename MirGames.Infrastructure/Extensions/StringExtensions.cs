// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="StringExtensions.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The MD5 hash.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns>The hash.</returns>
        public static string GetMd5Hash(this string s)
        {
            Contract.Requires(s != null);
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s));
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns true whether the specified strings have the same value.
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string..</param>
        /// <returns>True whether the specified strings have the same value.</returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}