namespace MirGames.Services.Git.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Services.Git.Entities;

    /// <summary>
    /// Mapping of the project RepositoryAccess.
    /// </summary>
    internal class RepositoryAccessMap : EntityTypeConfiguration<RepositoryAccess>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryAccessMap"/> class.
        /// </summary>
        public RepositoryAccessMap()
        {
            this.ToTable("git_repositories_access");
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("access_id");
            this.Property(t => t.AccessLevel).HasColumnName("level");
            this.Property(t => t.RepositoryId).HasColumnName("repository_id");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}
