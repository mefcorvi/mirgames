namespace MirGames.Domain.Entities
{
    using System;

    public partial class reminder
    {
        public string reminder_code { get; set; }
        public long user_id { get; set; }
        public System.DateTime reminder_date_add { get; set; }
        public Nullable<System.DateTime> reminder_date_used { get; set; }
        public System.DateTime reminder_date_expire { get; set; }
        public bool reminde_is_used { get; set; }
        public virtual User user { get; set; }
    }
}
