// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkdownSectionExtractor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.TextTransform
{
    using System.Linq;

    using Xilium.MarkdownDeep;

    /// <summary>
    /// The markdown short text extractor.
    /// </summary>
    public sealed class MarkdownSectionExtractor : ITextTransform
    {
        /// <inheritdoc />
        public string Transform(string text)
        {
            var userSections = Markdown.SplitUserSections(text).Where(s => !string.IsNullOrEmpty(s)).ToList();

            return userSections.Count > 1 ? userSections[0] : Markdown.SplitSections(text).First();
        }
    }
}