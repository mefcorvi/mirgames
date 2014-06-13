// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="InfrastructureModule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using Autofac;

    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Notifications;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Repositories;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Transactions;
    using MirGames.Infrastructure.Utilities;

    /// <summary>
    /// The infrastructure module.
    /// </summary>
    public class InfrastructureModule : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<QueryContextFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WriteContextFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QueryProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommandProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<NotificationManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RazorTemplateProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SearchEngine.SearchEngine>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CacheManagerFactory>().As<ICacheManagerFactory>().SingleInstance();
            builder.RegisterType<EventLog>().As<IEventLog>().SingleInstance();
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterType<ContentTypeProvider>().As<IContentTypeProvider>().SingleInstance();
            builder.RegisterType<TransactionExecutor>().As<ITransactionExecutor>().SingleInstance();
        }
    }
}
