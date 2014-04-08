namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The vote map.
    /// </summary>
    public class VoteMap : EntityTypeConfiguration<vote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteMap"/> class.
        /// </summary>
        public VoteMap()
        {
            // Primary Key
            this.HasKey(t => new { t.target_id, t.target_type, t.user_voter_id });

            // Properties
            this.Property(t => t.target_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.target_type).IsRequired().HasMaxLength(65532);

            this.Property(t => t.user_voter_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.vote_ip).IsRequired().HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("prefix_vote", "social");
            this.Property(t => t.target_id).HasColumnName("target_id");
            this.Property(t => t.target_type).HasColumnName("target_type");
            this.Property(t => t.user_voter_id).HasColumnName("user_voter_id");
            this.Property(t => t.vote_direction).HasColumnName("vote_direction");
            this.Property(t => t.vote_value).HasColumnName("vote_value");
            this.Property(t => t.vote_date).HasColumnName("vote_date");
            this.Property(t => t.vote_ip).HasColumnName("vote_ip");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.vote).HasForeignKey(d => d.user_voter_id);
        }
    }
}