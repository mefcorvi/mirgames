// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="OAuthController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Diagnostics.CodeAnalysis;
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
        /// The session manager.
        /// </summary>
        private readonly ISessionManager sessionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="sessionManager">The session manager.</param>
        public OAuthController(
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            ISessionManager sessionManager)
            : base(queryProcessor, commandProcessor)
        {
            this.sessionManager = sessionManager;
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
                    
                    return this.RedirectToAction("Settings", "Users");
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
                        this.sessionManager.SetSession(sessionId);
                        return this.RedirectToAction("Index", "Dashboard");
                    }
                }
            }

            return this.RedirectToAction("Index", "Dashboard");
        }

        /// <inheritdoc />
        [HttpPost]
        [AntiForgery]
        public ActionResult Authorize(string provider)
        {
            OAuthWebSecurity.RequestAuthentication(provider, Url.Action("Index", "OAuth"));
            return null;
        }
    }
}