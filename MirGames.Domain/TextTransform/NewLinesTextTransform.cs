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