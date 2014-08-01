// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MongoDatabaseFactory.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Services
{
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure;

    using MongoDB.Driver;

    /// <summary>
    /// Creates the instances of the Mongo Database.
    /// </summary>
    internal sealed class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDatabaseFactory"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MongoDatabaseFactory(ISettings settings)
        {
            Contract.Requires(settings != null);
            this.settings = settings;
        }

        /// <inheritdoc />
        public MongoDatabase CreateDatabase()
        {
            var connectionString = this.settings.GetValue("MongoDB.ConnectionString", "mongodb://localhost");
            var databaseName = this.settings.GetValue("MongoDB.Database", "mirgames");

            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            return server.GetDatabase(databaseName);
        }
    }
}