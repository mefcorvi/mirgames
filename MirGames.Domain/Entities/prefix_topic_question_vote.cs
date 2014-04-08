namespace MirGames.Domain.Entities
{
    public partial class topic_question_vote
    {
        public long topic_id { get; set; }
        public long user_voter_id { get; set; }
        public sbyte answer { get; set; }
        public virtual Topic topic { get; set; }
        public virtual User user { get; set; }
    }
}
