// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BlogMap.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// Mapping of the project Blog.
    /// </summary>
    internal sealed class BlogMap : EntityTypeConfiguration<Blog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogMap"/> class.
        /// </summary>
        public BlogMap()
        {
            this.ToTable("prefix_topic_blogs");
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("blog_id");
            this.Property(t => t.Alias).HasColumnName("alias").IsRequired();
            this.Property(t => t.Title).HasColumnName("title").IsRequired();
            this.Property(t => t.Description).HasColumnName("description").IsRequired();
        }
    }
}
