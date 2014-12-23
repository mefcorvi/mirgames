// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotificationsDomainModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Autofac;

    using MirGames.Domain.Notifications.Services;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Infrastructure;

    using MongoDB.Bson.Serialization;

    /// <summary>
    /// The notifications domain module.
    /// </summary>
    public sealed class NotificationsDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<NotificationTypeResolver>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MongoDatabaseFactory>().AsImplementedInterfaces().SingleInstance();

            var notifications = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("MirGames.Domain"))
                                         .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => new { assembly, type })
                                         .Where(@t => typeof(NotificationData).IsAssignableFrom(@t.type))
                                         .Select(@t => @t.type);

            foreach (var type in notifications)
            {
                MethodInfo method = typeof(BsonClassMap).GetMethod("RegisterClassMap", new Type[0]);
                MethodInfo generic = method.MakeGenericMethod(type);
                generic.Invoke(null, null);
            }
        }
    }
}
