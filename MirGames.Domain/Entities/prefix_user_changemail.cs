namespace MirGames.Domain.Entities
{
    using System;

    public partial class user_changemail
    {
        public int id { get; set; }
        public long user_id { get; set; }
        public System.DateTime date_add { get; set; }
        public Nullable<System.DateTime> date_used { get; set; }
        public System.DateTime date_expired { get; set; }
        public string mail_from { get; set; }
        public string mail_to { get; set; }
        public string code_from { get; set; }
        public string code_to { get; set; }
        public bool confirm_from { get; set; }
        public bool confirm_to { get; set; }
        public virtual User user { get; set; }
    }
}
