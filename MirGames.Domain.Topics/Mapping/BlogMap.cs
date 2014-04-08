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
