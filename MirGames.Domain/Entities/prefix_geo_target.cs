namespace MirGames.Domain.Entities
{
    using System;

    public partial class geo_target
    {
        public string geo_type { get; set; }
        public int geo_id { get; set; }
        public string target_type { get; set; }
        public int target_id { get; set; }
        public Nullable<int> country_id { get; set; }
        public Nullable<int> region_id { get; set; }
        public Nullable<int> city_id { get; set; }
        public virtual geo_city geo_city { get; set; }
        public virtual geo_country geo_country { get; set; }
        public virtual geo_region geo_region { get; set; }
    }
}
