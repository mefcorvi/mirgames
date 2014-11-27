// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WebModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.Reflection;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;

    using MirGames.Domain;
    using MirGames.Domain.Acl;
    using MirGames.Domain.Attachments;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users;
    using MirGames.Domain.Wip;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    using Mirgames.Pages;

    using MirGames.Services.Git;

    using Module = Autofac.Module;

    /// <summary>
    /// The web module.
    /// </summary>
    public sealed class WebModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();

            builder.RegisterInstance(RouteTable.Routes).As<RouteCollection>();

            builder.RegisterType<TextProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<AttachmentsDomainModule>();
            builder.RegisterModule<AclModule>();
            builder.RegisterModule<GitModule>();
            builder.RegisterModule<UsersDomainModule>();
            builder.RegisterModule<WipDomainModule>();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IEventListener>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientHostAddressProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }

    /// <summary>
    /// Not implemented text processor.
    /// </summary>
    public class TextProcessor : ITextProcessor
    {
        /// <inheritdoc />
        public string GetHtml(string source)
        {
            return string.Empty;
        }

        /// <inheritdoc />
        public string GetShortHtml(string source)
        {
            return string.Empty;
        }

        /// <inheritdoc />
        public string GetShortText(string source)
        {
            return string.Empty;
        }
    }
}
