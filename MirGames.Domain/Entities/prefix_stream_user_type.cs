namespace MirGames.Domain.Entities
{
    public partial class stream_user_type
    {
        public long user_id { get; set; }
        public string event_type { get; set; }
        public virtual User user { get; set; }
    }
}
