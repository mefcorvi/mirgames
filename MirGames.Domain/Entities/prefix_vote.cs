namespace MirGames.Domain.Entities
{
    using System;

    public partial class vote
    {
        public long target_id { get; set; }
        public string target_type { get; set; }
        public long user_voter_id { get; set; }
        public Nullable<sbyte> vote_direction { get; set; }
        public float vote_value { get; set; }
        public System.DateTime vote_date { get; set; }
        public string vote_ip { get; set; }
        public virtual User user { get; set; }
    }
}
