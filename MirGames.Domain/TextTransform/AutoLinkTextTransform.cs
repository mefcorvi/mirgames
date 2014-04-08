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