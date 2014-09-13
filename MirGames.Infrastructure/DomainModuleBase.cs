// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DomainModuleBase.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using Autofac;

    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Base class of the domain module.
    /// </summary>
    public abstract class DomainModuleBase : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var currentAssembly = this.GetType().Assembly;

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<ICommandHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IQueryHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IQueryHandlerDecorator>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IQueryCacheContainer>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IAccessRule>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IEventListener>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IAdditionalClaimsProvider>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}