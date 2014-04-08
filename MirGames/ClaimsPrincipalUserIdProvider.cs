namespace MirGames
{
    using System.Security.Claims;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Security;

    /// <summary>
    /// Provides an user ID for the SignalR request.
    /// </summary>
    internal sealed class ClaimsPrincipalUserIdProvider : IUserIdProvider
    {
        /// <inheritdoc />
        public string GetUserId(IRequest request)
        {
            var claimsPrincipal = request.User as ClaimsPrincipal;

            if (claimsPrincipal != null)
            {
                return claimsPrincipal.GetUserId().ToString();
            }

            return null;
        }
    }
}