namespace MirGames.Domain.Topics.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Topics.Entities;

    /// <summary>
    /// Mapping of the comment map.
    /// </summary>
    internal class CommentMap : EntityTypeConfiguration<Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentMap"/> class.
        /// </summary>
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            this.Property(t => t.Text)
                .IsOptional()
                .HasMaxLength(65535);

            this.Property(t => t.SourceText)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.UserLogin)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.UserIP)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("prefix_comment");
            this.Property(t => t.CommentId).HasColumnName("comment_id");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UserLogin).HasColumnName("user_login");
            this.Property(t => t.Text).HasColumnName("comment_text");
            this.Property(t => t.SourceText).HasColumnName("comment_source_text");
            this.Property(t => t.Date).HasColumnName("comment_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.UserIP).HasColumnName("comment_user_ip");
            this.Property(t => t.Rating).HasColumnName("comment_rating");

            this.HasRequired(t => t.Topic).WithMany().HasForeignKey(t => t.TopicId);
        }
    }
}
