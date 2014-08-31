// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkdownTextProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.Linq;

    using MirGames.Domain.TextTransform;

    /// <summary>
    /// The markdown text processor.
    /// </summary>
    internal sealed class MarkdownTextProcessor : ITextProcessor
    {
        /// <summary>
        /// The transform rules.
        /// </summary>
        private readonly ITextTransform[] transformRules =
        {
            new NewLinesTextTransform(),
            new AutoLinkTextTransform(),
            new UserLinkTextTransform(), 
            new MarkdownTextTransform()
        };

        /// <summary>
        /// The markdown section extractor.
        /// </summary>
        private readonly ITextTransform markdownSectionExtractor = new MarkdownSectionExtractor();

        /// <summary>
        /// The short text extractor.
        /// </summary>
        private readonly ITextTransform shortTextExtractor = new ShortTextExtractor();

        /// <inheritdoc />
        public string GetHtml(string source)
        {
            return this.transformRules.Aggregate(source, (current, transformRule) => transformRule.Transform(current));
        }

        /// <inheritdoc />
        public string GetShortHtml(string source)
        {
            return this.GetHtml(this.markdownSectionExtractor.Transform(source));
        }

        public string GetShortText(string source)
        {
            return this.shortTextExtractor.Transform(this.GetHtml(source));
        }
    }
}