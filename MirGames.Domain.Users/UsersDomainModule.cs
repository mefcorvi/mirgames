// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UsersDomainModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users
{
    using Autofac;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Domain.Users.Recaptcha;
    using MirGames.Domain.Users.Security;
    using MirGames.Domain.Users.Services;
    using MirGames.Domain.Users.UserSettings;
    using MirGames.Infrastructure;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class UsersDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UserAvatarUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<TimeZoneSetting>().As<IUserSettingHandler>().SingleInstance();
            builder.RegisterType<ThemeSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AuthenticationProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AvatarProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PrincipalCache>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<OnlineUsersManager>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<RecaptchaSettings>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RecaptchaVerificationProcessor>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
