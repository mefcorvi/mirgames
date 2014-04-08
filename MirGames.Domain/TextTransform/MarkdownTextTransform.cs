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