namespace MirGames.Domain.TextTransform
{
    using System.Linq;

    using HtmlAgilityPack;

    public sealed class ShortTextExtractor : ITextTransform
    {
        /// <inheritdoc />
        public string Transform(string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            var innerText = doc.DocumentNode.InnerText;

            if (innerText.Length > 143)
            {
                innerText =
                    new string(
                        innerText.Take(140)
                                 .Reverse()
                                 .SkipWhile(t => char.IsPunctuation(t) || char.IsWhiteSpace(t))
                                 .Reverse()
                                 .ToArray());

                innerText += "...";
            }

            return innerText;
        }
    }
}