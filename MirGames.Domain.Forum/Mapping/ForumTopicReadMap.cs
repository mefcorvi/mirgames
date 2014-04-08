namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The entity tag map.
    /// </summary>
    internal class ForumTopicReadMap : EntityTypeConfiguration<ForumTopicRead>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicReadMap"/> class.
        /// </summary>
        public ForumTopicReadMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("forum_topic_read");
            this.Property(t => t.Id).HasColumnName("topic_read_id");
            this.Property(t => t.StartTopicId).HasColumnName("start_topic_id");
            this.Property(t => t.EndTopicId).HasColumnName("end_topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}