namespace MirGames.Domain.Entities
{
    public partial class stream_subscribe
    {
        public long user_id { get; set; }
        public int target_user_id { get; set; }
        public virtual User user { get; set; }
    }
}
