namespace MirGames.Infrastructure.Security
{
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Implements the default text hash provider.
    /// </summary>
    internal sealed class TextHashProvider : ITextHashProvider
    {
        /// <inheritdoc />
        public string GetHash(string text)
        {
            Contract.Requires(text != null);
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}