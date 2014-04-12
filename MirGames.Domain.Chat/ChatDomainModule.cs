// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatDomainModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat
{
    using Autofac;

    using MirGames.Domain.Chat.Services;
    using MirGames.Domain.Chat.UserSettings;
    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class ChatDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UseEnterToSendChatMessageSetting>().As<IUserSettingHandler>().SingleInstance();
            builder.RegisterType<UseWebSocketSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<ChatMessageUploadProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
