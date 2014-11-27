// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Global.asax.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mirgames.Pages
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Autofac;

    using MirGames;

    using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <inheritdoc />
        protected void Application_Start()
        {
            this.InitContainer();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        private void InitContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WebModule>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
