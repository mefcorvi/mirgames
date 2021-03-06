﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WipDomainModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip
{
    using Autofac;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Domain.Wip.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class WipDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ProjectLogoUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<ProjectDescriptionUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<ProjectWorkItemUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<ProjectWorkItemCommentUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<ProjectGalleryUploadProcessor>().As<IUploadProcessor>().SingleInstance();
            builder.RegisterType<ProjectEmptyLogoProvider>().As<IProjectEmptyLogoProvider>().SingleInstance();
        }
    }
}
