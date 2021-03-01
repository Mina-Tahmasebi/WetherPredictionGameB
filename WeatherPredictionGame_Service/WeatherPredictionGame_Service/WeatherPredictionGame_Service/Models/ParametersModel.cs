using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherPredictionGame_Service.Models
{
    public class UserParameter
    {
        public string UserName { get; set; }
    }
    public class GameParameter
    {
        public int GameId { get; set; }
    }
    public class InitUserGuessParameter
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public List<Service.Models.UserGuessItemDto> LstUserGuessItemDto { get; set; }
    }
    public class CalculateGameParameter
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
    }

}
