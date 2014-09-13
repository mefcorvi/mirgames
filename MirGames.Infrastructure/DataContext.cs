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
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

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
        /// The properties.
        /// </summary>
        private static readonly IDictionary<Type, PropertyInfo[]> Properties = new Dictionary<Type, PropertyInfo[]>();

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
            : this(mappers, "Name=MirGames")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        public DataContext(IEnumerable<IEntityMapper> mappers, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Contract.Requires(mappers != null);
            this.mappers = mappers;
            this.Database.Log = s => TraceSource.TraceInformation(s);

            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ReadAllDateTimeValuesAsUtc;
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

        /// <summary>
        /// Reads all date time values as UTC.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ObjectMaterializedEventArgs"/> instance containing the event data.</param>
        private static void ReadAllDateTimeValuesAsUtc(object sender, ObjectMaterializedEventArgs e)
        {
            var entityType = e.Entity.GetType();
            if (!Properties.ContainsKey(entityType))
            {
                var entityProperties = entityType.GetProperties()
                    .Where(property => property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    .ToArray();

                Properties.Add(entityType, entityProperties);
            }

            Properties[entityType].ForEach(property => SpecifyUtcKind(property, e.Entity));
        }

        /// <summary>
        /// Specifies the kind of the UTC.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        private static void SpecifyUtcKind(PropertyInfo property, object value)
        {
            var datetime = property.GetValue(value, null);

            if (property.PropertyType == typeof(DateTime))
            {
                datetime = DateTime.SpecifyKind((DateTime)datetime, DateTimeKind.Utc);
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                var nullable = (DateTime?)datetime;
                
                if (!nullable.HasValue)
                {
                    return;
                }

                datetime = (DateTime?)DateTime.SpecifyKind(nullable.Value, DateTimeKind.Utc);
            }
            else
            {
                return;
            }

            property.SetValue(value, datetime, null);
        }
    }
}
