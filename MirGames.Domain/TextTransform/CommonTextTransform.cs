namespace MirGames.Domain.TextTransform
{
    using System.Linq;

    /// <summary>
    /// Represents logic of transformation of topic text.
    /// </summary>
    internal sealed class CommonTextTransform : ITextTransform
    {
        /// <inheritdoc />
        public string Transform(string text)
        {
            var transformRules = new ITextTransform[]
                {
                    new NewLinesTextTransform(), 
                    new AutoLinkTextTransform(),
                    new MarkdownTextTransform()
                };

            return transformRules.Aggregate(text, (current, transformRule) => transformRule.Transform(current));
        }
    }
}
