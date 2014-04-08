namespace MirGames.Domain.Forum.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Forum.Entities;

    /// <summary>
    /// The forum post mapping.
    /// </summary>
    internal sealed class ForumPostMap : EntityTypeConfiguration<ForumPost>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostMap"/> class.
        /// </summary>
        public ForumPostMap()
        {
            this.ToTable("forum_post");

            this.HasKey(t => t.PostId);

            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.AuthorLogin).HasColumnName("author_login");

            this.Property(t => t.AuthorIP)
                .IsRequired()
                .HasMaxLength(48)
                .HasColumnName("author_ip");

            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.IsHidden).HasColumnName("is_hidden");
            this.Property(t => t.PostId).HasColumnName("post_id");
            this.Property(t => t.SourceText).IsOptional().HasColumnName("source_text");
            this.Property(t => t.Text).IsRequired().HasColumnName("text");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.TopicId).HasColumnName("topic_id");
            this.Property(t => t.IsStartPost).HasColumnName("is_start_post");
            
            this.HasRequired(t => t.Topic).WithMany().HasForeignKey(t => t.TopicId);
        }
    }
}