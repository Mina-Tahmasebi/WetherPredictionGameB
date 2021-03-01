using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public virtual ICollection<CityGame> CityGames { get; set; }
    }
}
