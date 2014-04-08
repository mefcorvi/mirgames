namespace MirGames.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class talk
    {
        public talk()
        {
            this.talk_user = new List<talk_user>();
        }

        public long talk_id { get; set; }
        public long user_id { get; set; }
        public string talk_title { get; set; }
        public string talk_text { get; set; }
        public System.DateTime talk_date { get; set; }
        public System.DateTime talk_date_last { get; set; }
        public int talk_user_id_last { get; set; }
        public string talk_user_ip { get; set; }
        public Nullable<int> talk_comment_id_last { get; set; }
        public int talk_count_comment { get; set; }
        public virtual User user { get; set; }
        public virtual ICollection<talk_user> talk_user { get; set; }
    }
}
