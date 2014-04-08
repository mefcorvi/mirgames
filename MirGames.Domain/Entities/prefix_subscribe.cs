namespace MirGames.Domain.Entities
{
    using System;

    public partial class subscribe
    {
        public int id { get; set; }
        public string target_type { get; set; }
        public Nullable<int> target_id { get; set; }
        public string mail { get; set; }
        public System.DateTime date_add { get; set; }
        public Nullable<System.DateTime> date_remove { get; set; }
        public string ip { get; set; }
        public string key { get; set; }
        public bool status { get; set; }
    }
}
