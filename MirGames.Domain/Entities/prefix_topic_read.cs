namespace MirGames.Domain.Entities
{
    public partial class topic_read
    {
        public long topic_id { get; set; }
        public long user_id { get; set; }
        public System.DateTime date_read { get; set; }
        public long comment_count_last { get; set; }
        public int comment_id_last { get; set; }
        public virtual Topic topic { get; set; }
        public virtual User user { get; set; }
    }
}
