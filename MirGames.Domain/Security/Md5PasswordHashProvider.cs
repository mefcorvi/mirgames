namespace MirGames.Domain.Security
{
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Default implementation of password hash provider.
    /// </summary>
    internal sealed class Md5PasswordHashProvider : IPasswordHashProvider
    {
        /// <inheritdoc />
        public string GetHash(string password, string salt)
        {
            return (salt.GetMd5Hash() + password).GetMd5Hash();
        }
    }
}