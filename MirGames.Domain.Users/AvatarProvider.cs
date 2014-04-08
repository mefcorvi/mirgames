namespace MirGames.Domain.Users
{
    using MirGames.Infrastructure;

    /// <summary>
    /// Provides an URL to avatar.
    /// </summary>
    internal sealed class AvatarProvider : IAvatarProvider
    {
        /// <inheritdoc />
        public string GetAvatarUrl(string mail, string login)
        {
            return string.Format("http://www.gravatar.com/avatar/{0}?d=identicon", (mail ?? login).GetMd5Hash());
        }
    }
}