using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirGames.Domain.Tools
{
    using Autofac;

    public sealed class ToolsDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
