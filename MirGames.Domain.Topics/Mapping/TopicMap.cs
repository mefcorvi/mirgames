// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicMap.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// The topic map.
    /// </summary>
    internal class TopicMap : EntityTypeConfiguration<Topic>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicMap"/> class.
        /// </summary>
        public TopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TopicTitle).IsRequired().HasMaxLength(200);
            this.Property(t => t.TagsList).IsRequired().HasMaxLength(250);
            this.Property(t => t.UserIp).IsRequired().HasMaxLength(20);
            this.Property(t => t.CutText).HasMaxLength(100);
            this.Property(t => t.TextHash).IsRequired().HasMaxLength(32);
            this.Property(t => t.SourceAuthor).HasMaxLength(255);
            this.Property(t => t.SourceLink).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("prefix_topic");
            this.Property(t => t.Id).HasColumnName("topic_id");
            this.Property(t => t.AuthorId).HasColumnName("user_id");
            this.Property(t => t.BlogId).HasColumnName("blog_id");

            this.Property(t => t.TopicTitle).HasColumnName("topic_title");
            this.Property(t => t.TagsList).HasColumnName("topic_tags");
            this.Property(t => t.CreationDate).HasColumnName("topic_date_add");
            this.Property(t => t.UserIp).HasColumnName("topic_user_ip");
            this.Property(t => t.TextHash).HasColumnName("topic_text_hash");

            this.Property(t => t.TopicType).HasMaxLength(255).HasColumnName("topic_type").IsOptional();
            this.Property(t => t.EditDate).HasColumnName("topic_date_edit").IsOptional();
            this.Property(t => t.IsPublished).HasColumnName("topic_publish").IsOptional();
            this.Property(t => t.IsPublishedDraft).HasColumnName("topic_publish_draft").IsOptional();
            this.Property(t => t.IsPublishedIndex).HasColumnName("topic_publish_index").IsOptional();
            this.Property(t => t.Rating).HasColumnName("topic_rating").IsOptional();
            this.Property(t => t.CountVote).HasColumnName("topic_count_vote").IsOptional();
            this.Property(t => t.CountVoteUp).HasColumnName("topic_count_vote_up").IsOptional();
            this.Property(t => t.CountVoteDown).HasColumnName("topic_count_vote_down").IsOptional();
            this.Property(t => t.CountVoteAbstain).HasColumnName("topic_count_vote_abstain").IsOptional();
            this.Property(t => t.CountRead).HasColumnName("topic_count_read").IsOptional();
            this.Property(t => t.CountComment).HasColumnName("topic_count_comment").IsOptional();
            this.Property(t => t.CountFavorite).HasColumnName("topic_count_favourite").IsOptional();
            this.Property(t => t.CutText).HasColumnName("topic_cut_text").IsOptional();
            this.Property(t => t.ForbidComment).HasColumnName("topic_forbid_comment").IsOptional();
            this.Property(t => t.IsRepost).HasColumnName("moriginal_is_repost").IsOptional();
            this.Property(t => t.SourceAuthor).HasColumnName("moriginal_author").IsOptional();
            this.Property(t => t.SourceLink).HasColumnName("moriginal_link").IsOptional();
            this.Property(t => t.IsTutorial).HasColumnName("mtutorial").IsOptional();
            this.Property(t => t.ShowOnMain).HasColumnName("show_on_main");

            // Relationships
            this.HasOptional(t => t.Content).WithRequired(t => t.Topic).WillCascadeOnDelete();
        }
    }
}