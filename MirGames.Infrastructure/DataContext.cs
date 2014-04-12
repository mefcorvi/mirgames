// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DataContext.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure.Repositories;

    /// <summary>
    /// Represents the data context.
    /// </summary>
    internal class DataContext : DbContext
    {
        /// <summary>
        /// My trace source.
        /// </summary>
        private static readonly TraceSource TraceSource = new TraceSource("DataContext", SourceLevels.All);

        /// <summary>
        /// The entity mappers.
        /// </summary>
        private readonly IEnumerable<IEntityMapper> mappers;

        /// <summary>
        /// Initializes static members of the <see cref="DataContext"/> class.
        /// </summary>
        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public DataContext(IEnumerable<IEntityMapper> mappers)
            : base("Name=MirGames")
        {
            Contract.Requires(mappers != null);
            
            this.mappers = mappers;
            this.Database.Log = s => TraceSource.TraceInformation(s);
            
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized +=
                (sender, e) => DateTimeKindAttribute.Apply(e.Entity);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var registrty = new EntityMappingRegistry(modelBuilder);
            this.mappers.ForEach(m => m.Configure(registrty));

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
