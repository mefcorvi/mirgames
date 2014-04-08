namespace MirGames.Domain.TextTransform
{
    using System.Linq;

    using Xilium.MarkdownDeep;

    /// <summary>
    /// The markdown short text extractor.
    /// </summary>
    internal sealed class MarkdownShortTextExtractor : IShortTextExtractor
    {
        /// <inheritdoc />
        public string Extract(string text)
        {
            var userSections = Markdown.SplitUserSections(text).Where(s => !string.IsNullOrEmpty(s)).ToList();

            return userSections.Count > 1 ? userSections[0] : Markdown.SplitSections(text).First();
        }
    }
}