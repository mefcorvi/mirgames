// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewLinesTextTransform.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.TextTransform
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Converts links.
    /// </summary>
    internal sealed class NewLinesTextTransform : ITextTransform
    {
        /// <summary>
        /// The regular expression.
        /// </summary>
        private readonly Regex regularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewLinesTextTransform"/> class.
        /// </summary>
        public NewLinesTextTransform()
        {
            const string Regex = @"^[\w\<\>][^\n]*(\n+)";
            this.regularExpression = new Regex(Regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <inheritdoc />
        public string Transform(string text)
        {
            return this.regularExpression.Replace(
                text,
                m =>
                    {
                        if (m.Groups[1].Length > 1)
                        {
                            return m.Value;
                        }

                        return m.Value.TrimEnd() + "  \n";
                    });
        }
    }
}