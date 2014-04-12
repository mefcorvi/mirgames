// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumDomainModule.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum
{
    using Autofac;

    using MirGames.Domain.Forum.Services;
    using MirGames.Domain.Forum.UserSettings;
    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class ForumDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<UseContiniousPaginationSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<ForumPostUploadProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
