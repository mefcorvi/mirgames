﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Global.asax.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Microsoft.AspNet.SignalR;
    using Microsoft.Web.WebPages.OAuth;

    using MirGames.Controllers;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Commands;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;
    using MirGames.OAuth;

    using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

    /// <summary>
    /// The MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Handles the Start event of the Application.
        /// </summary>
        protected void Application_Start()
        {
            this.InitContainer();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            var authorizationHeader = this.Context.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var parts = authorizationHeader
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();

                if (parts.Length == 2 && parts[0].EqualsIgnoreCase("Basic"))
                {
                    string[] credentials =
                        Encoding.ASCII.GetString(Convert.FromBase64String(parts[1])).Split(new[] { ':' });

                    if (credentials.Length >= 2)
                    {
                        string login = credentials[0];
                        string password = string.Join(string.Empty, credentials.Skip(1));

                        var loginCommand = new LoginCommand
                        {
                            EmailOrLogin = login,
                            Password = password.GetMd5Hash()
                        };

                        var commandProcessor = (ICommandProcessor)DependencyResolver.Current.GetService(typeof(ICommandProcessor));
                        var sessionManager = (ISessionManager)DependencyResolver.Current.GetService(typeof(ISessionManager));
                        Thread.Sleep(3000); // we can execute login only once for a 3 seconds
                        var sessionId = commandProcessor.Execute(loginCommand);

                        sessionManager.SetSession(sessionId);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PostAuthenticateRequest event of the Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs args)
        {
            var controller = this.Context.Request.Url.GetRouteParameterValue("controller");
            if (controller == null || controller.EqualsIgnoreCase("WebApi"))
            {
                return;
            }

            var sessionCookie = this.Context.Request.Cookies["key"];
            string sessionId = sessionCookie != null ? sessionCookie.Value : string.Empty;

            var provider = DependencyResolver.Current.GetService<IAuthenticationProvider>();
            var principal = provider.GetPrincipal(sessionId);
            this.Context.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        /// <summary>
        /// Application_s the error.
        /// </summary>
        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            var httpRequest = HttpContext.Current.Request;

            if (IsNotFoundException(exception))
            {
                exception = new HttpException(404, string.Format("Page {0} not found", httpRequest.Url), exception);
            }

            var httpException = exception as HttpException;
            var eventLog = DependencyResolver.Current.GetService<IEventLog>();

            bool is404 = httpException != null && httpException.GetHttpCode() == 404;
            bool is500 = httpException != null && httpException.GetHttpCode() == 500;

            if (is500)
            {
                eventLog
                    .Log(
                        EventLogType.Error,
                        "Web",
                        httpException.InnerException.Message,
                        new
                        {
                            Exception = httpException.InnerException,
                            Url = httpRequest.Url.ToString(),
                            IP = httpRequest.UserHostAddress,
                            Referrer = httpRequest.UrlReferrer != null ? httpRequest.UrlReferrer.ToString() : string.Empty,
                            Browser = httpRequest.Browser.Browser
                        });
            }
            else
            {
                eventLog
                    .Log(
                        is404 ? EventLogType.Warning : EventLogType.Error,
                        "Web",
                        exception.Message,
                        new
                        {
                            Exception = exception,
                            Url = httpRequest.Url.ToString(),
                            IP = httpRequest.UserHostAddress,
                            Referrer = httpRequest.UrlReferrer != null ? httpRequest.UrlReferrer.ToString() : string.Empty,
                            Browser = httpRequest.Browser.Browser
                        });
            }

            if (DependencyResolver.Current.GetService<ISettings>().GetValue<bool>("Web.CustomErrorsEnabled", true))
            {
                this.ShowCustomErrorPage(exception);
            }
        }

        /// <summary>
        /// Determines whether the specified exception is not found exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>True whether the specified exception is not found exception.</returns>
        private static bool IsNotFoundException(Exception exception)
        {
            while (exception != null)
            {
                if (exception is ItemNotFoundException)
                {
                    return true;
                }

                exception = exception.InnerException;
            }

            return false;
        }

        /// <summary>
        /// Shows the custom error page.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);

            Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 404:
                    routeData.Values.Add("action", "Error404");
                    break;

                case 500:
                    routeData.Values.Add("action", "Error500");
                    break;

                default:
                    routeData.Values.Add("action", "GeneralError");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            Server.ClearError();

            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = httpException.GetHttpCode();

            var controller = (IController)DependencyResolver.Current.GetService(typeof(ErrorController));
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        private void InitContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WebModule>();

            var container = builder.Build();

            this.RegisterOAuthClients(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            this.ConfigureSignalR(container);
            this.ConfigureWebApi(container);
        }

        /// <summary>
        /// Registers the OAuth clients.
        /// </summary>
        /// <param name="container">The container.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private void RegisterOAuthClients(IContainer container)
        {
            var clientProviders = container.Resolve<IEnumerable<IAuthenticationClientProvider>>();
            clientProviders.ForEach(provider => OAuthWebSecurity.RegisterClient(provider.GetClient()));
        }

        /// <summary>
        /// Configures the SignalR.
        /// </summary>
        /// <param name="container">The container.</param>
        private void ConfigureSignalR(IContainer container)
        {
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
        }

        /// <summary>
        /// Configures the web API.
        /// </summary>
        /// <param name="container">The container.</param>
        private void ConfigureWebApi(IContainer container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}