// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="OAuthProviderMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The OAuth provider map.
    /// </summary>
    internal sealed class OAuthProviderMap : EntityTypeConfiguration<OAuthProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthProviderMap"/> class.
        /// </summary>
        public OAuthProviderMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_oauth_providers");
            this.Property(t => t.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
            this.Property(t => t.DisplayName).HasColumnName("display_name").IsRequired().HasMaxLength(255);
            this.Property(t => t.Id).HasColumnName("provider_id");
        }
    }
}