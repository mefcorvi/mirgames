namespace MirGames.Domain.Entities
{
    public partial class userfeed_subscribe
    {
        public long user_id { get; set; }
        public sbyte subscribe_type { get; set; }
        public int target_id { get; set; }
        public virtual User user { get; set; }
    }
}
