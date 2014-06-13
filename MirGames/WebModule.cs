// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WebModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MirGames.Services.Git;

namespace MirGames
{
    using System.Reflection;
    using System.Web.Routing;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.SignalR;
    using Autofac.Integration.WebApi;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain;
    using MirGames.Domain.Acl;
    using MirGames.Domain.Attachments;
    using MirGames.Domain.Chat;
    using MirGames.Domain.Forum;
    using MirGames.Domain.Tools;
    using MirGames.Domain.Topics;
    using MirGames.Domain.Users;
    using MirGames.Domain.Wip;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.OAuth;

    using RouteJs;

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
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();

            builder.RegisterInstance(RouteTable.Routes).As<RouteCollection>();
            builder.RegisterInstance(new ClaimsPrincipalUserIdProvider()).As<IUserIdProvider>();

            builder.RegisterModule<RouteJsModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<AclModule>();
            builder.RegisterModule<UsersDomainModule>();
            builder.RegisterModule<AttachmentsDomainModule>();
            builder.RegisterModule<ForumDomainModule>();
            builder.RegisterModule<TopicsDomainModule>();
            builder.RegisterModule<ChatDomainModule>();
            builder.RegisterModule<ToolsDomainModule>();
            builder.RegisterModule<WipDomainModule>();
            builder.RegisterModule<GitModule>();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IAuthenticationClientProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<IEventListener>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientHostAddressProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
