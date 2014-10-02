// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostVoteMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// Mapping of the forum post votes.
    /// </summary>
    internal sealed class ForumPostVoteMap : EntityTypeConfiguration<ForumPostVote>
    {
        public ForumPostVoteMap()
        {
            this.ToTable("forum_post_votes");

            this.HasKey(t => t.ForumPostVoteId);
            this.Property(t => t.ForumPostVoteId).HasColumnName("forum_post_vote_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.Vote).HasColumnName("vote");
            this.Property(t => t.PostId).HasColumnName("post_id");
        }
    }
}
