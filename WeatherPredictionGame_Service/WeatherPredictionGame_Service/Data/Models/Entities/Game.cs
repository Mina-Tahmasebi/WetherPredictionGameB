using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }
        public virtual ICollection<CityGame> CityGames { get; set; }
    }
}
