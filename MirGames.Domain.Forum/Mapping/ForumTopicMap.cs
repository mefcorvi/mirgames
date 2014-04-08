namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The forum topic mapping.
    /// </summary>
    internal sealed class ForumTopicMap : EntityTypeConfiguration<ForumTopic>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicMap"/> class.
        /// </summary>
        public ForumTopicMap()
        {
            this.ToTable("forum_topic");

            this.HasKey(t => t.TopicId);

            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.AuthorLogin).HasColumnName("author_login");

            this.Property(t => t.AuthorIp)
                .IsRequired()
                .HasMaxLength(48)
                .HasColumnName("author_ip");

            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.LastPostAuthorId).HasColumnName("last_post_author_id");
            this.Property(t => t.LastPostAuthorLogin).HasColumnName("last_post_author_login");
            this.Property(t => t.PostsCount).HasColumnName("posts_count");
            this.Property(t => t.TagsList).IsRequired().HasMaxLength(1024).HasColumnName("tags_list");
            this.Property(t => t.Title).IsRequired().HasMaxLength(1024).HasColumnName("title");

            this.HasMany(t => t.Posts).WithRequired(t => t.Topic).HasForeignKey(t => t.TopicId).WillCascadeOnDelete();
        }
    }
}