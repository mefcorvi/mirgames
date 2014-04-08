namespace MirGames.Domain.Entities
{
    public partial class stream_event
    {
        public int id { get; set; }
        public string event_type { get; set; }
        public int target_id { get; set; }
        public long user_id { get; set; }
        public System.DateTime date_added { get; set; }
        public bool publish { get; set; }
        public virtual User user { get; set; }
    }
}
