// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AccountController.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Exceptions;
    using MirGames.Domain.Users.Queries;
    using MirGames.Filters;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Exception;

    using Recaptcha.MvcModel;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public AccountController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <returns>The action result.</returns>
        [AjaxOnly]
        public ActionResult Login()
        {
            var authProviders = this.QueryProcessor.Process(new GetOAuthProvidersQuery());

            return this.PartialView("_LoginDialog", authProviders);
        }

        /// <summary>
        /// The login action.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult ProcessLogin(LoginCommand command)
        {
            command.Password = command.Password.GetMd5Hash();
            string sessionId = this.CommandProcessor.Execute(command);

            if (string.IsNullOrEmpty(sessionId))
            {
                return this.Json(new { result = 0 });
            }

            this.SetSession(sessionId);

            return this.Json(new { result = 1 });
        }

        /// <summary>
        /// The logout action.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User, ReadOnlyUser")]
        public ActionResult Logout()
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
        public ActionResult SignUp()
        {
            return this.PartialView("_SignUpDialog", new SignUpCommand());
        }

        /// <inheritdoc />
        public ActionResult Activation(string key)
        {
            string sessionId = this.CommandProcessor.Execute(new ActivateUserCommand { ActivationKey = key });

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return this.HttpNotFound("Wrong activation key");
            }

            this.SetSession(sessionId);
            return this.RedirectToAction("Index", "Dashboard");
        }

        /// <inheritdoc />
        public ActionResult RestorePassword(string key)
        {
            this.CommandProcessor.Execute(new RestorePasswordCommand { SecretKey = key });
            return this.RedirectToAction("Index", "Dashboard");
        }

        /// <inheritdoc />
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult LoginAs(LoginAsUserCommand command)
        {
            string sessionId = this.CommandProcessor.Execute(command);

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return this.Json(new { result = 1 });
            }

            this.SetSession(sessionId);
            return this.Json(new { result = 0 });
        }

        /// <inheritdoc />
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult SaveSettings(SaveAccountSettingsCommand command)
        {
            this.CommandProcessor.Execute(command);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// The sign up action.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public ActionResult ProcessSignUp(SignUpCommand command)
        {
            command.Password = command.Password.GetMd5Hash();

            if (!this.Request.ValidateCaptcha().IsValid)
            {
                return this.Json(new { result = "WrongCaptcha" });
            }

            try
            {
                this.CommandProcessor.Execute(command);
            }
            catch (CommandProcessorException e)
            {
                if (e.InnerException is UserAlreadyRegisteredException)
                {
                    this.ViewData.Add("Error", "Пользователь с таким логином или адресом электронной почты уже зарегистрирован.");
                    return this.Json(new { result = "AlreadyRegistered" });
                }

                throw;
            }

            string sessionId = this.CommandProcessor.Execute(
                new LoginCommand
                    {
                        EmailOrLogin = command.Login,
                        Password = command.Password
                    });

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                this.ViewData.Add("Error", "Не удалось войти под пользователем");
                return this.Json(new { result = "LoginFailed" });
            }

            this.SetSession(sessionId);
            return this.Json(new { result = "Success" });
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
                        Expires = DateTime.UtcNow.AddYears(1)
                    });
        }
    }
}
