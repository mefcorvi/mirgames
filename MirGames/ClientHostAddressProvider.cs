namespace MirGames
{
    using System.Web;

    using MirGames.Infrastructure;

    internal sealed class ClientHostAddressProvider : IClientHostAddressProvider
    {
        /// <inheritdoc />
        public string GetHostAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}