// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AclModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl
{
    using Autofac;

    using MirGames.Domain.Acl.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class AclModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ActionTypesResolver>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityTypesResolver>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PermissionsCacheManager>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
