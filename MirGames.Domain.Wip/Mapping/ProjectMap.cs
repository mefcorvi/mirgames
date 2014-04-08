namespace MirGames.Domain.Wip.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Wip.Entities;

    /// <summary>
    /// Mapping of the project entity.
    /// </summary>
    internal class ProjectMap : EntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectMap"/> class.
        /// </summary>
        public ProjectMap()
        {
            this.ToTable("wip_projects");
            this.HasKey(t => t.ProjectId);

            this.Property(t => t.ProjectId).HasColumnName("project_id");
            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.Alias).HasColumnName("alias").IsRequired();
            this.Property(t => t.CreationDate).HasColumnName("creation_date");
            this.Property(t => t.Description).HasColumnName("description").IsRequired();
            this.Property(t => t.FollowersCount).HasColumnName("followers_count");
            this.Property(t => t.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
            this.Property(t => t.TagsList).HasColumnName("tags_list").IsRequired().HasMaxLength(1024);
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.Version).HasColumnName("version").IsOptional().HasMaxLength(255);
            this.Property(t => t.Votes).HasColumnName("votes");
            this.Property(t => t.VotesCount).HasColumnName("votes_count");
            this.Property(t => t.RepositoryId).HasColumnName("repository_id").IsOptional();
            this.Property(t => t.RepositoryType).HasColumnName("repository_type").IsOptional();
            this.Property(t => t.BlogId).HasColumnName("blog_id").IsOptional();
        }
    }
}
