namespace MirGames.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    using DotNetOpenAuth.GoogleOAuth2;

    using Microsoft.Web.WebPages.OAuth;

    using MirGames.Domain.Users.Commands;
    using MirGames.Infrastructure;

    /// <summary>
    /// OAuth controller.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class OAuthController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public OAuthController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult Index()
        {
            GoogleOAuth2Client.RewriteRequest();
            var authenticationResult = OAuthWebSecurity.VerifyAuthentication(Url.Action("Index", "OAuth"));

            if (authenticationResult.IsSuccessful)
            {
                if (this.CurrentUser != null)
                {
                    this.CommandProcessor.Execute(
                        new AttachOAuthProviderCommand
                            {
                                ProviderName = authenticationResult.Provider,
                                ProviderUserId = authenticationResult.ProviderUserId,
                                Data = authenticationResult.ExtraData
                            });
                    
                    return RedirectToAction("Settings", "Users");
                }

                if (this.CurrentUser == null)
                {
                    string sessionId = this.CommandProcessor.Execute(
                        new OAuthLoginCommand
                            {
                                ProviderName = authenticationResult.Provider,
                                ProviderUserId = authenticationResult.ProviderUserId
                            });

                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        this.SetSession(sessionId);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        /// <inheritdoc />
        [HttpPost]
        [AntiForgery]
        public ActionResult Authorize(string provider)
        {
            OAuthWebSecurity.RequestAuthentication(provider, Url.Action("Index", "OAuth"));
            return null;
        }

        /// <summary>
        /// Sets the session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        private void SetSession(string sessionId)
        {
            this.HttpContext.Response.Cookies.Add(
                new HttpCookie("key", sessionId)
                {
                    Path = "/",
                    Expires = DateTime.UtcNow.AddDays(7)
                });
        }
    }
}