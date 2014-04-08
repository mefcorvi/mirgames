namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class ForumTopicUnreadMap : EntityTypeConfiguration<ForumTopicUnread>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicUnreadMap"/> class.
        /// </summary>
        public ForumTopicUnreadMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("forum_topic_unread");
            this.Property(t => t.Id).HasColumnName("topic_unread_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UnreadDate).HasColumnName("unread_date");
        }
    }
}