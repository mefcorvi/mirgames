// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkdownTextTransform.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain
{
    using MirGames.Domain.TextTransform;

    using Xilium.MarkdownDeep;

    /// <summary>
    /// Transforms the markdown to the html.
    /// </summary>
    internal sealed class MarkdownTextTransform : ITextTransform
    {
        /// <inheritdoc />
        public string Transform(string text)
        {
            var markdown = new Markdown
                {
                    SafeMode = true,
                    NoFollowLinks = true,
                    NewWindowForExternalLinks = true,
                    NewWindowForLocalLinks = false,
                    UserBreaks = true,
                    ExtraMode = true,
                    AutoHeadingIDs = false
                };

            return markdown.Transform(text);
        }
    }
}