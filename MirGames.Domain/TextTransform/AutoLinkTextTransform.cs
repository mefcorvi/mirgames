// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AutoLinkTextTransform.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.TextTransform
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Converts links.
    /// </summary>
    internal sealed class AutoLinkTextTransform : ITextTransform
    {
        /// <summary>
        /// The regular expression.
        /// </summary>
        private readonly Regex regularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoLinkTextTransform"/> class.
        /// </summary>
        public AutoLinkTextTransform()
        {
            const string Regex = @"(\[[\s\S]*\]\()?((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?#%&~\-_]*)([""'#!\(\)?, ><;])?(\))?";
            this.regularExpression = new Regex(Regex, RegexOptions.IgnoreCase);
        }

        /// <inheritdoc />
        public string Transform(string text)
        {
            return this.regularExpression.Replace(
                text,
                m =>
                    {
                        if (m.Groups[1].Length > 0 && m.Value.EndsWith(")"))
                        {
                            return m.Value;
                        }

                        var link = string.Format("({0})", m.Groups[2].Value).Replace("(www", "(http://www");

                        return string.Format("{3}[{0}]{1}{2}", m.Groups[2].Value, link, m.Groups[5].Value, m.Groups[1].Value);
                    });
        }
    }
}