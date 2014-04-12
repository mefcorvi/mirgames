// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityMapper.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users
{
    using MirGames.Domain.Users.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new UserSessionMap());
            registry.Register(new UserMap());
            registry.Register(new PasswordRestoreRequestMap());

            registry.Register(new UserAdministratorMap());
            registry.Register(new WallRecordMap());

            registry.Register(new OAuthTokenMap());
            registry.Register(new OAuthTokenDataMap());
            registry.Register(new OAuthProviderMap());
        }
    }
}