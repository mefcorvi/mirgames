// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityMappingRegistry.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The entity mapping registry.
    /// </summary>
    internal sealed class EntityMappingRegistry : IEntityMappingRegistry
    {
        /// <summary>
        /// The model builder.
        /// </summary>
        private readonly DbModelBuilder modelBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMappingRegistry"/> class.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public EntityMappingRegistry(DbModelBuilder modelBuilder)
        {
            Contract.Requires(modelBuilder != null);
            this.modelBuilder = modelBuilder;
        }

        /// <summary>
        /// Registers mapping in the registry.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <typeparam name="T">Type of entity.</typeparam>
        public void Register<T>(EntityTypeConfiguration<T> configuration) where T : class
        {
            Contract.Requires(configuration != null);
            this.modelBuilder.Configurations.Add(configuration);
        }
    }
}