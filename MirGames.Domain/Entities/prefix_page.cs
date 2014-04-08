namespace MirGames.Domain.Entities
{
    using System;

    public partial class page
    {
        public long page_id { get; set; }
        public Nullable<long> page_pid { get; set; }
        public string page_url { get; set; }
        public string page_url_full { get; set; }
        public string page_title { get; set; }
        public string page_text { get; set; }
        public System.DateTime page_date_add { get; set; }
        public Nullable<System.DateTime> page_date_edit { get; set; }
        public string page_seo_keywords { get; set; }
        public string page_seo_description { get; set; }
        public bool page_active { get; set; }
        public bool page_main { get; set; }
        public int page_sort { get; set; }
        public bool page_auto_br { get; set; }
    }
}
