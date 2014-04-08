namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The topic_question_vote map.
    /// </summary>
    public class TopicQuestionVoteMap : EntityTypeConfiguration<topic_question_vote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicQuestionVoteMap"/> class.
        /// </summary>
        public TopicQuestionVoteMap()
        {
            // Primary Key
            this.HasKey(t => new { t.topic_id, t.user_voter_id, t.answer });

            // Properties
            this.Property(t => t.topic_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_voter_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_topic_question_vote", "social");
            this.Property(t => t.topic_id).HasColumnName("topic_id");
            this.Property(t => t.user_voter_id).HasColumnName("user_voter_id");
            this.Property(t => t.answer).HasColumnName("answer");

            // Relationships
            this.HasRequired(t => t.topic).WithMany(t => t.topic_question_vote).HasForeignKey(d => d.topic_id);
            this.HasRequired(t => t.user).WithMany(t => t.topic_question_vote).HasForeignKey(d => d.user_voter_id);
        }
    }
}