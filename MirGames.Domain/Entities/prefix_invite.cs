namespace MirGames.Domain.Entities
{
    using System;

    public partial class invite
    {
        public long invite_id { get; set; }
        public string invite_code { get; set; }
        public long user_from_id { get; set; }
        public Nullable<long> user_to_id { get; set; }
        public System.DateTime invite_date_add { get; set; }
        public Nullable<System.DateTime> invite_date_used { get; set; }
        public bool invite_used { get; set; }
        public virtual User user { get; set; }
        public virtual User user1 { get; set; }
    }
}
