// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="OAuthTokenMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// Map for OAuth token.
    /// </summary>
    internal sealed class OAuthTokenMap : EntityTypeConfiguration<OAuthToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthTokenMap"/> class.
        /// </summary>
        public OAuthTokenMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("user_oauth_tokens");

            this.Property(t => t.ProviderId).HasColumnName("provider_id");
            this.Property(t => t.Id).HasColumnName("token_id");
            this.Property(t => t.ProviderUserId).HasColumnName("provider_user_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}