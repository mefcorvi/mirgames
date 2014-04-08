namespace MirGames.Domain.Entities
{
    using System.Collections.Generic;

    public partial class geo_city
    {
        public geo_city()
        {
            this.geo_target = new List<geo_target>();
        }

        public int id { get; set; }
        public int country_id { get; set; }
        public int region_id { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
        public int sort { get; set; }
        public virtual geo_country geo_country { get; set; }
        public virtual geo_region geo_region { get; set; }
        public virtual ICollection<geo_target> geo_target { get; set; }
    }
}
