// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatMessageMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Chat.Entities;

    /// <summary>
    /// The forum post mapping.
    /// </summary>
    internal sealed class ChatMessageMap : EntityTypeConfiguration<ChatMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageMap"/> class.
        /// </summary>
        public ChatMessageMap()
        {
            this.ToTable("chat_messages");

            this.HasKey(t => t.MessageId);

            this.Property(t => t.MessageId).HasColumnName("message_id");
            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.AuthorLogin).HasColumnName("author_login");
            this.Property(t => t.AuthorIp).IsRequired().HasMaxLength(48).HasColumnName("author_ip");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.Message).IsRequired().HasColumnName("message");
        }
    }
}