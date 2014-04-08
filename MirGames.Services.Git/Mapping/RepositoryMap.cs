namespace MirGames.Services.Git.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Services.Git.Entities;

    /// <summary>
    /// Mapping of the project Repository.
    /// </summary>
    internal class RepositoryMap : EntityTypeConfiguration<Repository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryMap"/> class.
        /// </summary>
        public RepositoryMap()
        {
            this.ToTable("git_repositories");
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("repository_id");
            this.Property(t => t.Name).HasColumnName("name").IsRequired();
            this.Property(t => t.Title).HasColumnName("title").IsRequired();
        }
    }
}
