namespace MirGames.Domain.Entities
{
    public partial class user_note
    {
        public int id { get; set; }
        public long target_user_id { get; set; }
        public long user_id { get; set; }
        public string text { get; set; }
        public System.DateTime date_add { get; set; }
        public virtual User user { get; set; }
        public virtual User user1 { get; set; }
    }
}
