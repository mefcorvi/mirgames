// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DomainModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain
{
    using System.Security.Claims;

    using Autofac;

    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Settings;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class DomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Md5PasswordHashProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TextHashProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<MarkdownTextProcessor>().AsImplementedInterfaces().SingleInstance();
            
            builder.Register(c => ClaimsPrincipal.Current).As<ClaimsPrincipal>().InstancePerDependency();
            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>().SingleInstance();

            builder.RegisterType<AppSettings>().Named<ISettings>("settings").SingleInstance();

            builder.RegisterDecorator<ISettings>(
                (c, inner) => new DataStoredSettings(inner, c.Resolve<IReadContextFactory>()), fromKey: "settings", toKey: "dataStoredSettings");

            builder.RegisterDecorator<ISettings>(
                (c, inner) => new CachedSettings(inner, c.Resolve<ICacheManagerFactory>().Create("Common")), fromKey: "dataStoredSettings");
        }
    }
}
