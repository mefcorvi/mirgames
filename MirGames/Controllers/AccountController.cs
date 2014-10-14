// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AccountController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Filters;
    using MirGames.Infrastructure;

    /// <summary>
    /// The account controller.
    /// </summary>
    public partial class AccountController : AppController
    {
        /// <summary>
        /// The session manager.
        /// </summary>
        private readonly ISessionManager sessionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="sessionManager">The session manager.</param>
        public AccountController(
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            ISessionManager sessionManager)
            : base(queryProcessor, commandProcessor)
        {
            Contract.Requires(sessionManager != null);

            this.sessionManager = sessionManager;
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <returns>The action result.</returns>
        [AjaxOnly]
        public virtual ActionResult Login()
        {
            var authProviders = this.QueryProcessor.Process(new GetOAuthProvidersQuery());

            return this.PartialView("_LoginDialog", authProviders);
        }

        /// <summary>
        /// The logout action.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User, ReadOnlyUser")]
        public virtual ActionResult Logout()
        {
            this.CommandProcessor.Execute(new LogoutCommand());

            var keyCookie = this.HttpContext.Response.Cookies["key"];

            if (keyCookie != null)
            {
                keyCookie.Expires = DateTime.UtcNow.AddMinutes(-1);
            }

            return this.RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        /// The sign up action.
        /// </summary>
        /// <returns>The action result.</returns>
        [AjaxOnly]
        public virtual ActionResult SignUp()
        {
            return this.PartialView("_SignUpDialog", new SignUpCommand());
        }

        /// <inheritdoc />
        public virtual ActionResult Activation(string key)
        {
            string sessionId = this.CommandProcessor.Execute(new ActivateUserCommand { ActivationKey = key });

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return this.HttpNotFound("Wrong activation key");
            }

            this.sessionManager.SetSession(sessionId);
            return this.RedirectToAction("Index", "Dashboard");
        }

        /// <inheritdoc />
        public virtual ActionResult RestorePassword(string key)
        {
            this.CommandProcessor.Execute(new RestorePasswordCommand { SecretKey = key });
            return this.RedirectToAction("Index", "Dashboard");
        }
    }
}
