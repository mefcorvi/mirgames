namespace MirGames.Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The MD5 hash.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <returns>The hash.</returns>
        public static string GetMd5Hash(this string s)
        {
            Contract.Requires(s != null);
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s));
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns true whether the specified strings have the same value.
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string..</param>
        /// <returns>True whether the specified strings have the same value.</returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}